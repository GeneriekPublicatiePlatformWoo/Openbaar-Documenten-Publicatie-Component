<template>
  <fieldset>
    <legend>Document</legend>

    <alert-inline v-if="error"
      >Er is iets misgegaan bij het ophalen van het document...</alert-inline
    >

    <section v-else-if="publicatieDocument">
      <template v-if="publicatieDocument.uuid">
        <dl>
          <dt>Titel</dt>
          <dd>{{ publicatieDocument.officieleTitel }}</dd>

          <dt>Verkorte titel</dt>
          <dd>{{ publicatieDocument.verkorteTitel }}</dd>

          <dt>Omschrijving</dt>
          <dd>{{ publicatieDocument.omschrijving || "-" }}</dd>

          <dt>Bestandsnaam</dt>
          <dd>{{ publicatieDocument.bestandsnaam }}</dd>
        </dl>

        <button @click="reset" class="button secondary icon-after trash">Verwijder document</button>
      </template>

      <template v-else>
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

          <input
            id="bestand"
            type="file"
            title="Voeg bestand toe"
            required
            aria-required="true"
            @change="onFileSelected"
          />
        </div>
      </template>
    </section>
  </fieldset>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap, uploadDocument } from "../service";

const { uuid } = defineProps<{ uuid?: string }>();

const emit = defineEmits<{
  (e: "update:loading", payload: boolean): void;
  (e: "update:error", payload: boolean): void;
}>();

defineExpose({ upload });

const file = ref<File | null>();

const initialDocument = (): PublicatieDocument => ({
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

const publicatieDocument = ref<PublicatieDocument | null>(initialDocument());

const { data, isFetching, error, post, execute } = useFetchApi(
  () => `/api-mock/v1/documenten${uuid ? "/" + uuid : ""}`,
  { immediate: false }
).json<PublicatieDocument>();

watch(data, (value) => (publicatieDocument.value = value), { immediate: false });

watch(isFetching, (value) => emit("update:loading", value));

watch(error, (value) => emit("update:error", !!value));

const onFileSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;

  if (target.files === null) return;

  file.value = target.files[0];

  const bestandsformaat = mimeTypesMap.value?.get(file.value.type)?.identifier;

  if (!publicatieDocument.value || !bestandsformaat) {
    toast.add({
      text: "Het document is van een onbekend type, probeer het nogmaals...",
      type: "error"
    });

    target.value = "";
    return;
  }

  publicatieDocument.value.bestandsnaam = file.value.name;
  publicatieDocument.value.bestandsomvang = file.value.size;
  publicatieDocument.value.bestandsformaat = bestandsformaat;
};

async function upload(publicatie: string): Promise<void> {
  if (!publicatieDocument.value || publicatieDocument.value.uuid) return;

  publicatieDocument.value.publicatie = publicatie;

  // Metadata
  post(publicatieDocument);

  await execute();

  if (error.value) {
    toast.add({
      text: "De metadata bij het document kon niet worden opgeslagen, probeer het nogmaals...",
      type: "error"
    });

    error.value = null;

    throw new Error();
  }

  // File
  if (file.value && publicatieDocument.value.bestandsdelen?.length) {
    emit("update:loading", true);

    try {
      await uploadDocument(file.value, publicatieDocument.value.bestandsdelen);
    } catch {
      toast.add({
        text: "Het document kon niet worden geupload, probeer het nogmaals...",
        type: "error"
      });

      reset();

      throw new Error();
    } finally {
      emit("update:loading", false);
    }
  }
}

const reset = () => (publicatieDocument.value = initialDocument());

onMounted(async () => uuid && (await execute()));
</script>

<style lang="scss" scoped>
button {
  display: flex;
  column-gap: var(--spacing-small);
}

dl {
  margin-block: 0;

  dt {
    font-weight: var(--font-bold);
  }

  dd {
    margin-inline-start: 0;
    margin-block-end: var(--spacing-default);
  }
}
</style>
