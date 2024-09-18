<template>
  <h2>{{ gebruikersgroep?.name }}</h2>

  <simple-spinner v-if="loading"></simple-spinner>

  <form v-else aria-live="polite" @submit.prevent="submit">
    <alert-inline v-if="error"> Er is iets misgegaan, probeer het nogmaals... </alert-inline>

    <section v-else-if="gebruikersgroep">
      <CheckboxList
        v-for="(value, key) in WAARDELIJSTEN"
        :key="key"
        :title="value"
        :options="groupedWaardelijstItems[key]"
        v-model="waardelijstenModel"
      />
    </section>

    <div class="form-submit" :class="{ error }">
      <router-link :to="{ name: 'gebruikersgroepen' }" class="button">{{
        error ? "&lt; Terug" : "Annuleren"
      }}</router-link>

      <button v-if="gebruikersgroep && !error" type="submit" title="Opslaan">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { computed, ref, watchEffect } from "vue";
import { useRouter } from "vue-router";
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";
import CheckboxList from "@/components/CheckboxList.vue";

import type { Gebruikersgroep } from "./types";
import { WAARDELIJSTEN, getWaardelijsten } from "@/features/waardelijst";

const router = useRouter();
const props = defineProps<{ id: string }>();

const loading = computed<boolean>(() => loadingListItems.value || loadingGebruikersgroep.value);
const error = computed<boolean>(() => listItemstError.value || gebruikersgroepError.value);

const { groupedWaardelijstItems, waardelijstIds, loadingListItems, listItemstError } =
  getWaardelijsten();

const waardelijstenModel = ref<string[]>([]);

const {
  data: gebruikersgroep,
  isFetching: loadingGebruikersgroep,
  error: gebruikersgroepError,
  put,
  execute
} = useFetchApi(() => `/api/gebruikersgroepen/${props.id}`).json<Gebruikersgroep>();

watchEffect(() => {
  waardelijstenModel.value = gebruikersgroep.value?.gekoppeldeWaardelijsten || [];
});

const submit = async (): Promise<void> => {
  const formData = {
    gekoppeldeWaardelijsten: waardelijstenModel.value?.filter((id) =>
      waardelijstIds.value.includes(id)
    )
  };

  put(formData);

  await execute();

  toast.add(
    gebruikersgroepError.value
      ? { text: "De gegevens konden niet worden opgeslagen.", type: "error" }
      : { text: "De gegevens zijn succesvol opgeslagen." }
  );

  !error.value && router.push({ name: "gebruikersgroepen" });
};
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width-small), 1fr));
  grid-gap: var(--spacing-default);
}
</style>
