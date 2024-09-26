<template>
  <simple-spinner v-show="loading"></simple-spinner>

  <form v-show="!loading" aria-live="polite" @submit.prevent="submit">
    <section>
      <alert-inline v-if="publicatieError"
        >Er is iets misgegaan bij het ophalen van de publicatie...</alert-inline
      >

      <publicatie-form v-else v-model="publicatie" />

      <alert-inline v-if="documentenError"
        >Er is iets misgegaan bij het ophalen van de documenten...</alert-inline
      >

      <document-form
        v-else
        v-model:documenten="documenten"
        v-model:files="files"
        @removeDocument="removeDocument"
      />
    </section>

    <div class="form-submit">
      <router-link :to="{ name: 'publicaties' }" class="button button-secondary"
        >Annuleren</router-link
      >

      <button type="submit" title="Opslaan" :disabled="error">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useRouter } from "vue-router";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";
import PublicatieForm from "./components/PublicatieForm.vue";
import DocumentForm from "./components/DocumentForm.vue";
import { usePublicatie } from "./use-publicatie";
import { useDocumenten } from "./use-documenten";

const router = useRouter();

const { uuid } = defineProps<{ uuid?: string }>();

const loading = computed(
  () =>
    loadingPublicatie.value ||
    loadingDocumenten.value ||
    loadingDocument.value ||
    uploadingFiles.value
);

const error = computed(() => publicatieError.value || documentenError.value);

// Publicatie
const { publicatie, publicatieError, loadingPublicatie, submitPublicatie } = usePublicatie(uuid);

// Documenten
const {
  files,
  documenten,
  loadingDocumenten,
  documentenError,
  loadingDocument,
  uploadingFiles,
  submitDocumenten,
  removeDocument
} = useDocumenten(computed(() => uuid || publicatie.value?.uuid));

// Submit
const submit = async (): Promise<void> => {
  try {
    await submitPublicatie();
    
    await submitDocumenten();
  } catch {
    return;
  }

  toast.add({ text: "De publicatie is succesvol opgeslagen." });
  router.push({ name: "publicaties" });
};
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));
  grid-gap: var(--spacing-default);
}

button {
  display: flex;
  column-gap: var(--spacing-small);
}
</style>
