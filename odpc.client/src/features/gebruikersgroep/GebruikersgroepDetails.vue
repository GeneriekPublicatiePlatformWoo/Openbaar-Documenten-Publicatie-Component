<template>
  <simple-spinner v-if="loading"></simple-spinner>

  <form v-show="!loading" @submit.prevent="submit" ref="formRef" novalidate>
    <alert-inline v-if="error"
      >Er is iets misgegaan bij het ophalen van de gegevens...</alert-inline
    >

    <section v-else-if="gebruikersgroep">
      <gebruikersgroep-form v-model="gebruikersgroep" />

      <fieldset>
        <legend>Waardelijsten</legend>

        <checkbox-list
          v-for="(value, key) in WAARDELIJSTEN"
          :key="key"
          :title="value"
          :options="groupedWaardelijstItems[key]"
          v-model="gebruikersgroep.gekoppeldeWaardelijsten"
        />
      </fieldset>
    </section>

    <div class="form-submit">
      <span class="required-message">Velden met (*) zijn verplicht</span>

      <router-link :to="{ name: 'gebruikersgroepen' }" class="button button-secondary"
        >Annuleren</router-link
      >

      <button type="submit" title="Opslaan" :disabled="error">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue";
import { useRouter } from "vue-router";
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import CheckboxList from "@/components/CheckboxList.vue";
import toast from "@/stores/toast";
import { validateForm } from "@/helpers/validate";
import type { Gebruikersgroep } from "./types";
import GebruikersgroepForm from "./components/GebruikersgroepForm.vue";
import { WAARDELIJSTEN, getWaardelijsten } from "@/features/waardelijst";

const router = useRouter();

const { uuid } = defineProps<{ uuid?: string }>();

const formRef = ref<HTMLFormElement>();
const loading = computed<boolean>(() => loadingListItems.value || loadingGebruikersgroep.value);
const error = computed<boolean>(() => !!listItemstError.value || !!gebruikersgroepError.value);

const { groupedWaardelijstItems, waardelijstIds, loadingListItems, listItemstError } =
  getWaardelijsten();

const gebruikersgroep = ref<Gebruikersgroep>({
  naam: "",
  omschrijving: "",
  gekoppeldeWaardelijsten: []
});

const {
  get: getGebruikersgroep,
  post: postGebruikersgroep,
  put: putGebruikersgroep,
  data: gebruikersgroepData,
  isFetching: loadingGebruikersgroep,
  error: gebruikersgroepError
} = useFetchApi(() => `/api/gebruikersgroepen${uuid ? "/" + uuid : ""}`, {
  immediate: false
}).json<Gebruikersgroep>();

watch(gebruikersgroepData, (value) => (gebruikersgroep.value = value || gebruikersgroep.value), {
  immediate: false
});

const submit = async (): Promise<void> => {
  if (validateForm(formRef.value).invalid) return;

  gebruikersgroep.value = {
    ...gebruikersgroep.value,
    gekoppeldeWaardelijsten: gebruikersgroep.value.gekoppeldeWaardelijsten.filter((id) =>
      waardelijstIds.value.includes(id)
    )
  };

  uuid
    ? await putGebruikersgroep(gebruikersgroep).execute()
    : await postGebruikersgroep(gebruikersgroep).execute();

  toast.add(
    gebruikersgroepError.value
      ? {
          text: "De gegevens konden niet worden opgeslagen, probeer het nogmaals...",
          type: "error"
        }
      : { text: "De gegevens zijn succesvol opgeslagen." }
  );

  if (!gebruikersgroepError.value) {
    // redirect
    router.push({ name: "gebruikersgroepen" });
  } else {
    // retry
    gebruikersgroepError.value = null;
  }
};

onMounted(async () => uuid && (await getGebruikersgroep().execute()));
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));
  grid-gap: var(--spacing-default);
}
</style>
