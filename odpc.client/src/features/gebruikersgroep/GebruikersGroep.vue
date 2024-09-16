<template>
  <h2>{{ gebruikersgroep?.name }}</h2>

  <simple-spinner v-if="loading"></simple-spinner>

  <form v-else aria-live="polite" @submit.prevent="submit">
    <alert-inline v-if="error">
      Er is iets misgegaan bij het ophalen van de waardelijsten, probeer het nogmaals...
    </alert-inline>

    <section v-else-if="gebruikersgroep">
      <CheckboxList
        v-for="(value, key) in WAARDELIJSTEN"
        :key="key"
        :title="value"
        :options="groupedItems[key]"
        v-model="waardelijstenModel"
      />
    </section>

    <div class="form-submit">
      <router-link :to="{ name: 'gebruikersgroepen' }" class="button">&lt; Terug</router-link>

      <button v-if="!error && gebruikersgroep" type="submit" title="Opslaan">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { computed, ref, watchEffect } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";
import CheckboxList from "@/components/CheckboxList.vue";

import type { Gebruikersgroep } from "@/features/gebruikersgroep/types";
import { getWaardelijsten } from "@/features/waardelijst/service";
import { WAARDELIJSTEN } from "@/features/waardelijst/types";

const props = defineProps<{ id: string }>();
const loading = computed<boolean>(() => loadingListItems.value || loadingGebruikersgroep.value);
const error = computed<boolean>(() => listItemstError.value || gebruikersgroepError.value);

const { groupedItems, loadingListItems, listItemstError } = getWaardelijsten();

const waardelijstenModel = ref<string[]>([]);

const {
  data: gebruikersgroep,
  isFetching: loadingGebruikersgroep,
  error: gebruikersgroepError,
  put,
  execute
} = useFetchApi(`/gebruikersgroepen/${props.id}`).json<Gebruikersgroep>();

watchEffect(() => {
  waardelijstenModel.value = gebruikersgroep.value?.gekoppeldeWaardelijsten || [];
});

const formData = computed(() => ({
  gekoppeldeWaardelijsten: waardelijstenModel.value?.filter((id) =>
    Object.values(groupedItems.value)
      .flat()
      .map((item) => item.id)
      .includes(id)
  )
}));

const submit = async (): Promise<void> => {
  put(formData);
  await execute();

  toast.add(
    gebruikersgroepError.value
      ? { text: "De gegevens konden niet worden opgeslagen.", type: "error" }
      : { text: "De gegevens zijn succesvol opgeslagen." }
  );
};
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width-small), 1fr));
  grid-gap: var(--spacing-default);

  // ...
  padding: 0;
  margin: 0;
  border: none;
}

.form-submit {
  display: flex;
  justify-content: flex-end;
  grid-gap: var(--spacing-default);
}
</style>
