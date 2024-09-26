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
      >
        <button @click.prevent="addDocument" class="button secondary icon-after plus">
          Document toevoegen
        </button>
      </document-form>
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
import { ref, computed, watch, onMounted } from "vue";
import { useRouter } from "vue-router";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";
import PublicatieForm from "./components/PublicatieForm.vue";
import DocumentForm from "./components/DocumentForm.vue";
import { useFetchApi } from "@/api/use-fetch-api";
import { useDocumenten } from "./use-documenten";
import type { Publicatie } from "./types";

const router = useRouter();

const { uuid } = defineProps<{ uuid?: string }>();

const uploading = ref(false);

const loading = computed(
  () =>
    loadingPublicatie.value || loadingDocumenten.value || loadingDocument.value || uploading.value
);

const error = computed(() => publicatieError.value || documentenError.value);

// Publicatie
const publicatie = ref<Publicatie>({
  officieleTitel: "",
  verkorteTitel: "",
  omschrijving: "",
  creatiedatum: new Date().toISOString().split("T")[0]
});

const {
  get: getPublicatie,
  post: postPublicatie,
  put: putPublicatie,
  data: publicatieData,
  isFetching: loadingPublicatie,
  error: publicatieError
} = useFetchApi(() => `/api/v1/publicaties${uuid ? "/" + uuid : ""}`, {
  immediate: false
}).json<Publicatie>();

watch(publicatieData, (value) => (publicatie.value = value || publicatie.value), {
  immediate: false
});

// Documenten
const {
  files,
  documenten,
  documentenError,
  loadingDocumenten,
  loadingDocument,
  submitDocument,
  uploadDocument,
  addDocument,
  removeDocument
} = useDocumenten(uuid || publicatie.value?.uuid);

// Submit
const submitPublicatie = async (): Promise<void> => {
  uuid ? await putPublicatie(publicatie).execute() : await postPublicatie(publicatie).execute();

  if (publicatieError.value) {
    toast.add({
      text: "De publicatie kon niet worden opgeslagen, probeer het nogmaals...",
      type: "error"
    });

    publicatieError.value = null;

    throw new Error();
  }
};

const submit = async (): Promise<void> => {
  try {
    await submitPublicatie();

    await submitDocument();

    await uploadDocument();
  } catch {
    return;
  }

  toast.add({ text: "De publicatie is succesvol opgeslagen." });

  router.push({ name: "publicaties" });
};

onMounted(async () => uuid && (await getPublicatie().execute()));
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
