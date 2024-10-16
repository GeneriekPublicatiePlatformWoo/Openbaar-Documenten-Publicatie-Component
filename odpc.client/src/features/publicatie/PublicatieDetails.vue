<template>
  <simple-spinner v-show="loading"></simple-spinner>

  <form v-show="!loading" @submit.prevent="submit" ref="formRef" novalidate>
    <section>
      <alert-inline v-if="publicatieError"
        >Er is iets misgegaan bij het ophalen van de publicatie...</alert-inline
      >

      <publicatie-form
        v-else
        v-model="publicatie"
        :disabled="status === PublicatieStatus.ingetrokken"
      />

      <alert-inline v-if="documentenError"
        >Er is iets misgegaan bij het ophalen van de documenten...</alert-inline
      >

      <document-form
        v-else
        v-model:documenten="documenten"
        v-model:files="files"
        @removeDocument="removeDocument"
        :disabled="status === PublicatieStatus.ingetrokken"
      />
    </section>

    <div class="form-submit">
      <span class="required-message">Velden met (*) zijn verplicht</span>

      <menu class="reset">
        <li>
          <button type="button" title="Opslaan" class="button secondary" @click="navigate">
            Annuleren
          </button>
        </li>

        <li>
          <button
            type="submit"
            title="Opslaan"
            :disabled="error || status === PublicatieStatus.ingetrokken"
          >
            Opslaan
          </button>
        </li>
      </menu>
    </div>

    <prompt-modal
      :dialog="dialog"
      confirm-message="Ja, intrekken"
      cancel-message="Nee, gepubliceerd laten"
    >
      <span>Weet u zeker dat u dit deze publicatie wilt intrekken?</span>
      <span><strong>Let op:</strong> deze actie kan niet ongedaan worden gemaakt.</span>
    </prompt-modal>
  </form>
</template>

<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { previousRoute } from "@/router";
import { useConfirmDialog } from "@vueuse/core";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import PromptModal from "@/components/PromptModal.vue";
import toast from "@/stores/toast";
import { validateForm } from "@/helpers/validate";
import PublicatieForm from "./components/PublicatieForm.vue";
import DocumentForm from "./components/DocumentForm.vue";
import { usePublicatie } from "./composables/use-publicatie";
import { useDocumenten } from "./composables/use-documenten";
import { PublicatieStatus } from "./types";

const router = useRouter();

const props = defineProps<{ uuid?: string }>();

const formRef = ref<HTMLFormElement>();

const dialog = useConfirmDialog();

const loading = computed(
  () =>
    loadingPublicatie.value ||
    loadingDocumenten.value ||
    loadingDocument.value ||
    uploadingFile.value
);

const error = computed(() => !!publicatieError.value || !!documentenError.value);

// Publicatie
const status = ref<keyof typeof PublicatieStatus>(PublicatieStatus.gepubliceerd);

const { publicatie, publicatieError, loadingPublicatie, submitPublicatie } = usePublicatie(
  props.uuid
);

watch(
  () => publicatie.value.status,
  (value) => (status.value = value),
  { once: true }
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
  removeDocument
} =
  // Get associated docs by uuid prop when existing pub, so no need to wait for pub fetch.
  // Publicatie.uuid is used when new pub and associated docs: docs submit waits for pub submit/publicatie.uuid.
  useDocumenten(computed(() => props.uuid || publicatie.value?.uuid));

const navigate = () => {
  if (previousRoute.value?.name === "publicaties") {
    router.push({ name: previousRoute.value.name, query: previousRoute.value?.query });
  } else {
    router.push({ name: "publicaties" });
  }
};

const submit = async () => {
  if (validateForm(formRef.value).invalid) return;

  if (publicatie.value.status === PublicatieStatus.ingetrokken) {
    const { isCanceled } = await dialog.reveal();

    if (isCanceled) {
      publicatie.value.status = PublicatieStatus.gepubliceerd;

      return;
    }
  }

  try {
    await submitPublicatie();

    await submitDocumenten();
  } catch {
    return;
  }

  toast.add({ text: "De publicatie is succesvol opgeslagen." });

  navigate();
};
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));
  grid-gap: var(--spacing-default);
}
</style>
