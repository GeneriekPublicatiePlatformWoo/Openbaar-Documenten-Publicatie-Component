<template>
  <fieldset v-if="publicatie">
    <legend>Publicatie</legend>

    <div class="form-group">
      <label for="titel">Titel</label>

      <input
        id="titel"
        type="text"
        v-model="publicatie.officieleTitel"
        required
        aria-required="true"
      />
    </div>

    <div class="form-group">
      <label for="verkorte_titel">Verkorte titel</label>

      <input id="verkorte_titel" type="text" v-model="publicatie.verkorteTitel" />
    </div>

    <div class="form-group">
      <label for="omschrijving">Omschrijving</label>

      <textarea id="omschrijving" v-model="publicatie.omschrijving" rows="4"></textarea>
    </div>

    <!-- <pre>{{ model }}</pre> -->
  </fieldset>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import toast from "@/stores/toast";
import type { Publicatie } from "../types";

const { uuid } = defineProps<{ uuid?: string; loading: boolean }>();
const emit = defineEmits<{ (e: "update:loading", payload: boolean): void }>();
defineExpose({ submit });

const publicatie = ref<Publicatie | null>({
  officieleTitel: "",
  verkorteTitel: "",
  omschrijving: "",
  creatiedatum: new Date().toISOString().split("T")[0]
});

const { data, isFetching, error, post, put, execute } = useFetchApi(
  () => `/api-mock/v1/publicaties${uuid ? "/" + uuid : ""}`,
  { immediate: false }
).json<Publicatie>();

watch(data, (value) => (publicatie.value = value), { immediate: false });
watch(isFetching, (value) => emit("update:loading", value));

async function submit(): Promise<string> {
  uuid ? put(publicatie) : post(publicatie);

  await execute();

  if (error.value || !data.value?.uuid) {
    toast.add({
      text: "De publicatie kon niet worden opgeslagen, probeer het nogmaals...",
      type: "error"
    });

    throw new Error();
  }

  return data.value.uuid;
}

onMounted(async () => uuid && (await execute()));
</script>

<style lang="scss" scoped></style>
