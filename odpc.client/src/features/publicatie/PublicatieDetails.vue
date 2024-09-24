<template>
  <simple-spinner v-show="loading"></simple-spinner>

  <form v-show="!loading" aria-live="polite" @submit.prevent="submit">
    <section v-if="publicatie && publicatieDocument">
      <publicatie-form v-model="publicatie" />

      <document-form
        v-model:publicatieDocument="publicatieDocument"
        v-model:file="file"
        @reset="reset"
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
import { ref, computed, watch, onMounted } from "vue";
import { useRouter } from "vue-router";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import toast from "@/stores/toast";
import PublicatieForm from "./components/PublicatieForm.vue";
import DocumentForm from "./components/DocumentForm.vue";
import type { Publicatie, PublicatieDocument } from "./types";
import { useFetchApi } from "@/api/use-fetch-api";
import { uploadDocument } from "./service";

const router = useRouter();

const { uuid } = defineProps<{ uuid?: string }>();

const loading = computed(() => loadingPublicatie.value || loadingDocument.value);
const error = computed(() => publicatieError.value || documentError.value);

// Publicatie
const publicatie = ref<Publicatie | null>({
  officieleTitel: "",
  verkorteTitel: "",
  omschrijving: "",
  creatiedatum: new Date().toISOString().split("T")[0]
});

const {
  data: publicatieData,
  isFetching: loadingPublicatie,
  error: publicatieError,
  post: postPublicatie,
  put: putPublicatie,
  execute: executePublicatie
} = useFetchApi(() => `/api-mock/v1/publicaties${uuid ? "/" + uuid : ""}`, {
  immediate: false
}).json<Publicatie>();

watch(publicatieData, (value) => (publicatie.value = value), { immediate: false });

// Document
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

const reset = () => (publicatieDocument.value = initialDocument());

const {
  data: documentData,
  isFetching: loadingDocument,
  error: documentError,
  post: postDocument,
  execute: executeDocument
} = useFetchApi(() => `/api-mock/v1/documenten${uuid ? "/" + uuid : ""}`, {
  immediate: false
}).json<PublicatieDocument>();

watch(documentData, (value) => (publicatieDocument.value = value), { immediate: false });

const submit = async (): Promise<void> => {
  uuid ? putPublicatie(publicatie) : postPublicatie(publicatie);

  await executePublicatie();

  if (publicatieError.value || !publicatieData.value?.uuid) {
    toast.add({
      text: "De publicatie kon niet worden opgeslagen, probeer het nogmaals...",
      type: "error"
    });

    publicatieError.value = null;

    return;
  }

  if (!publicatie.value?.uuid || !publicatieDocument.value || publicatieDocument.value.uuid) return;

  publicatieDocument.value.publicatie = publicatie.value?.uuid;

  // Metadata
  postDocument(publicatieDocument);

  await executeDocument();

  if (error.value) {
    toast.add({
      text: "De metadata bij het document kon niet worden opgeslagen, probeer het nogmaals...",
      type: "error"
    });

    documentError.value = null;

    return;
  }

  // File
  if (file.value && documentData.value?.bestandsdelen?.length) {
    // start loading

    try {
      await uploadDocument(file.value, documentData.value.bestandsdelen);
    } catch {
      toast.add({
        text: "Het document kon niet worden geupload, probeer het nogmaals...",
        type: "error"
      });

      reset();

      return;
    } finally {
      // stop loading
    }
  }

  toast.add({ text: "De publicatie is succesvol opgeslagen." });

  router.push({ name: "publicaties" });
};

onMounted(async () => uuid && (await executePublicatie()) && (await executeDocument()));
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));
  grid-gap: var(--spacing-default);
}
</style>
