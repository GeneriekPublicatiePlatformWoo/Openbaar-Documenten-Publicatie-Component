<template>
  <simple-spinner v-show="loading"></simple-spinner>

  <form v-show="!loading" aria-live="polite" @submit.prevent="submit">
    <section>
      <publicatie-form
        ref="publicatieFormRef"
        :uuid="uuid"
        v-model:loading="loadingPublicatie"
        v-model:error="publicatieError"
      />

      <document-form
        ref="documentFormRef"
        :uuid="uuid"
        v-model:loading="loadingDocument"
        v-model:error="documentError"
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
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import toast from "@/stores/toast";
import PublicatieForm from "./components/PublicatieForm.vue";
import DocumentForm from "./components/DocumentForm.vue";

const router = useRouter();

const { uuid } = defineProps<{ uuid?: string }>();

const loading = computed(() => loadingPublicatie.value || loadingDocument.value);
const error = computed(() => publicatieError.value || documentError.value);

const loadingPublicatie = ref<boolean>(false);
const publicatieError = ref<boolean>(false);
const publicatieFormRef = ref<InstanceType<typeof PublicatieForm> | null>(null);

const loadingDocument = ref<boolean>(false);
const documentError = ref<boolean>(false);
const documentFormRef = ref<InstanceType<typeof DocumentForm> | null>(null);

const submit = async (): Promise<void> => {
  const responseUuid = await publicatieFormRef.value?.submit().catch(() => null);

  if (!responseUuid) return;

  try {
    await documentFormRef.value?.upload(responseUuid);
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
