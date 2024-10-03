<template>
  <simple-spinner v-show="loading"></simple-spinner>

  <form v-show="!loading" @submit.prevent="submit" ref="formRef" novalidate>
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
        @toggleDocument="toggleDocument"
      />
    </section>

    <div class="form-submit">
      <span class="required-message">Velden met (*) zijn verplicht</span>

      <router-link :to="{ name: 'publicaties' }" class="button button-secondary"
        >Annuleren</router-link
      >

      <button type="submit" title="Opslaan" :disabled="error">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useRouter } from "vue-router";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";
import { validateForm } from "@/helpers/validate";
import PublicatieForm from "./components/PublicatieForm.vue";
import DocumentForm from "./components/DocumentForm.vue";
import { usePublicatie } from "./use-publicatie";
import { useDocumenten } from "./use-documenten";

const router = useRouter();

const props = defineProps<{ uuid?: string }>();

const formRef = ref<HTMLFormElement>();

const loading = computed(
  () =>
    loadingPublicatie.value ||
    loadingDocumenten.value ||
    loadingDocument.value ||
    uploadingFile.value
);

const error = computed(() => !!publicatieError.value || !!documentenError.value);

// Publicatie
const { publicatie, publicatieError, loadingPublicatie, submitPublicatie } = usePublicatie(
  props.uuid
);

// Documenten
const {
  files,
  documenten,
  loadingDocumenten,
  documentenError,
  loadingDocument,
  uploadingFile,
  submitDocumenten,
  removeDocument,
  toggleDocument
} =
  // Get associated docs by uuid prop when existing pub, so no need to wait for pub fetch.
  // Publicatie.uuid is used when new pub and associated docs: docs submit waits for pub submit/publicatie.uuid.
  useDocumenten(computed(() => props.uuid || publicatie.value?.uuid));

const submit = async (): Promise<void> => {
  if (validateForm(formRef.value).invalid) return;

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
</style>
