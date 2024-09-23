<template>
  <fieldset v-if="publicatieDocument">
    <legend>Document</legend>

    <div class="form-group">
      <label for="titel">Titel</label>

      <input
        id="titel"
        type="text"
        v-model="publicatieDocument.officieleTitel"
        required
        aria-required="true"
      />
    </div>

    <div class="form-group">
      <label for="verkorte_titel">Verkorte titel</label>

      <input id="verkorte_titel" type="text" v-model="publicatieDocument.verkorteTitel" />
    </div>

    <div class="form-group">
      <label for="omschrijving">Omschrijving</label>

      <textarea id="omschrijving" v-model="publicatieDocument.omschrijving" rows="4"></textarea>
    </div>

    <div class="form-group">
      <label for="bestand">Bestand toevoegen</label>

      <input id="bestand" type="file" title="Voeg bestand toe" @change="onFileSelected" />
    </div>

    <pre>{{ publicatieDocument }}</pre>
  </fieldset>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap, uploadDocument } from "../service";

const { uuid } = defineProps<{ uuid?: string; loading: boolean }>();
const emit = defineEmits<{ (e: "update:loading", payload: boolean): void }>();
defineExpose({ upload });

const file = ref<File | null>();

const publicatieDocument = ref<PublicatieDocument | null>({
  publicatie: "",
  officieleTitel: "",
  verkorteTitel: "",
  omschrijving: "",
  creatiedatum: new Date().toISOString().split("T")[0],
  bestandsnaam: "",
  bestandsformaat: "",
  bestandsomvang: 0,
  bestandsdelen: []
});

const { data, isFetching, error, post, put, execute } = useFetchApi(
  () => `/api-mock/v1/documenten${uuid ? "/" + uuid : ""}`,
  { immediate: false }
).json<PublicatieDocument>();

watch(data, (value) => (publicatieDocument.value = value), { immediate: false });
watch(isFetching, (value) => emit("update:loading", value));

const onFileSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;

  if (target.files === null) return;

  file.value = target.files[0];

  if (publicatieDocument.value) {
    publicatieDocument.value.bestandsnaam = file.value.name;
    publicatieDocument.value.bestandsformaat =
      mimeTypesMap.value?.get(file.value.type)?.identifier || "onbekend";
    publicatieDocument.value.bestandsomvang = file.value.size;
  }
};

async function upload(publicatie: string): Promise<void> {
  if (publicatieDocument.value) {
    publicatieDocument.value.publicatie = publicatie;
  }

  uuid ? put(publicatieDocument) : post(publicatieDocument);

  await execute();

  if (error.value) {
    toast.add({
      text: "De meta data bij het document kon niet worden opgeslagen, probeer het nogmaals...",
      type: "error"
    });

    throw new Error();
  }

  const bestandsdelen = data.value?.bestandsdelen || [];

  if (file.value && bestandsdelen.length) {
    emit("update:loading", true);

    try {
      await uploadDocument(file.value, bestandsdelen);
    } catch {
      toast.add({
        text: "Het document kon niet worden geupload, probeer het nogmaals...",
        type: "error"
      });

      throw new Error();
    } finally {
      emit("update:loading", false);
    }
  }
}

onMounted(async () => uuid && (await execute()));
</script>

<style lang="scss" scoped></style>
