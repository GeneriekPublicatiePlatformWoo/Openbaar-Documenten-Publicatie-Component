<template>
  <simple-spinner v-show="isFetching"></simple-spinner>

  <form  v-show="!isFetching" aria-live="polite" @submit.prevent="submit">
    <alert-inline v-if="error"
      >Er is iets misgegaan bij het ophalen van de publicatie...</alert-inline
    >

    <section v-else-if="publicatie">
      <publicatie-form v-model="publicatie" />

      <file-upload-form ref="fileUpload" />
    </section>

    <div class="form-submit" :class="{ error }">
      <router-link :to="{ name: 'publicaties' }" class="button button-secondary">{{
        error ? "&lt; Terug" : "Annuleren"
      }}</router-link>

      <button v-if="publicatie && !error" type="submit" title="Opslaan">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";
import PublicatieForm from "./components/PublicatieForm.vue";
import FileUploadForm from "./components/FileUploadForm.vue";

import type { Publicatie } from "./types";

const router = useRouter();
const props = defineProps<{ id?: string }>();

const fileUpload = ref<{ submit: (publicatie: string) => Promise<string> }>();

const publicatie = ref<Publicatie | null>({
  officieleTitel: "",
  verkorteTitel: "",
  omschrijving: "",
  creatiedatum: new Date().toISOString().split("T")[0]
});

const { data, isFetching, error, post, put, execute } = useFetchApi(
  () => `/api-mock/v1/publicaties${props.id ? "/" + props.id : ""}`,
  { immediate: false }
).json<Publicatie>();

watch(data, () => (publicatie.value = data.value), { immediate: false });

const submit = async (): Promise<void> => {
  props.id ? put(publicatie) : post(publicatie);

  await execute();

  toast.add(
    error.value
      ? { text: "De publicatie kon niet worden opgeslagen, probeer het nogmaals...", type: "error" }
      : { text: "De publicatie is succesvol opgeslagen." }
  );

  if (!error.value) {
    // redirect
    router.push({ name: "publicaties" });
  } else {
    // retry
    error.value = null;
  }
};

onMounted(async () => props.id && (await execute()));
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));
  grid-gap: var(--spacing-default);
}
</style>
