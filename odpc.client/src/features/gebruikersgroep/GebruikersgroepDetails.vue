<template>
  <simple-spinner v-show="loadingGebruikersgroep"></simple-spinner>

  <default-form v-show="!loadingGebruikersgroep" @submit.prevent="submit">
    <section>
      <alert-inline v-if="gebruikersgroepError"
        >Er is iets misgegaan bij het ophalen van de gebruikersgroep...</alert-inline
      >

      <gebruikersgroep-form v-else v-model="gebruikersgroep" />

      <waardelijsten-form v-model="gebruikersgroep.gekoppeldeWaardelijsten" />
    </section>

    <div class="form-submit">
      <span class="required-message">Velden met (*) zijn verplicht</span>

      <menu class="reset">
        <li>
          <router-link :to="{ name: 'gebruikersgroepen' }" class="button button-secondary"
            >Annuleren</router-link
          >
        </li>

        <li v-if="uuid">
          <button
            type="button"
            title="Verwijderen"
            class="secondary"
            :disabled="gebruikersgroepError"
            @click="remove"
          >
            Verwijderen
          </button>
        </li>

        <li>
          <button type="submit" title="Opslaan" :disabled="gebruikersgroepError">Opslaan</button>
        </li>
      </menu>
    </div>

    <prompt-modal :dialog="dialog" confirm-message="Ja, verwijderen" cancel-message="Nee, behouden">
      <p>Weet u zeker dat u deze gebruikersgroep wilt verwijderen?</p>
    </prompt-modal>
  </default-form>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router";
import { useConfirmDialog } from "@vueuse/core";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import PromptModal from "@/components/PromptModal.vue";
import DefaultForm from "@/components/DefaultForm.vue";
import toast from "@/stores/toast";
import GebruikersgroepForm from "./components/GebruikersgroepForm.vue";
import WaardelijstenForm from "./components/WaardelijstenForm.vue";
import { useGebruikersgroep } from "./composables/use-gebruikersgroep";

const router = useRouter();

const props = defineProps<{ uuid?: string }>();

const dialog = useConfirmDialog();

const {
  gebruikersgroep,
  loadingGebruikersgroep,
  gebruikersgroepError,
  submitGebruikersgroep,
  removeGebruikersgroep
} = useGebruikersgroep(props.uuid);

const submit = async () => {
  try {
    await submitGebruikersgroep();
  } catch {
    return;
  }

  toast.add({ text: "De gegevens zijn succesvol opgeslagen." });
  router.push({ name: "gebruikersgroepen" });
};

const remove = async () => {
  const { isCanceled } = await dialog.reveal();

  if (isCanceled) return;

  try {
    await removeGebruikersgroep();
  } catch {
    return;
  }

  toast.add({ text: "De gebruikersgroep is succesvol verwijderd." });
  router.push({ name: "gebruikersgroepen" });
};
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));
  grid-gap: var(--spacing-default);
}
</style>
