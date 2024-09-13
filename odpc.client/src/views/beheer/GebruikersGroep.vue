<template>
  <h1>Gebruikersgroep &gt; {{ gebruikersgroep?.name }}</h1>

  <simple-spinner v-if="loading"></simple-spinner>

  <form v-else aria-live="polite" @submit.prevent="submit">
    <alert-view v-if="error">
      Er is iets misgegaan bij het ophalen van de waardelijsten, probeer het nogmaals...
    </alert-view>


    <section v-else-if="gebruikersgroep">


      <WaardelijstView v-for="(value, key) in WAARDELIJSTEN" :key="key" :title="value" :items="groupedItems[key]"
        v-model="waardelijstenModel" />
    </section>

    <div class="form-submit">
      <router-link :to="{ name: 'gebruikersgroepen' }" class="button">&lt; Terug</router-link>

      <button v-if="!error" type="submit" title="Opslaan">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { computed, ref, watchEffect } from "vue";
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertView from "@/components/AlertView.vue";
import toast from "@/stores/toast";

import {
  WAARDELIJSTEN,
  type Waardelijst,
  type WaardelijstItem,
  type GroupedWaardeLijstItems
} from "@/../mock/api.mock";
import WaardelijstView from "@/components/WaardeLijstView.vue";

const props = defineProps<{
  id: string;
}>();

const loading = computed<boolean>(() => loadingListItems.value || loadingGebruikersgroep.value);
const error = computed<boolean>(() => listItemstError.value || gebruikersgroepError.value);

type Gebruikersgroep = {
  id: string,
  name: string,
  gekoppeldeWaardelijsten: Array<string>
}

const {
  data: gebruikersgroep,
  isFetching: loadingGebruikersgroep,
  error: gebruikersgroepError,
  put,
  execute
} = useFetchApi(`/gebruikersgroepen/${props.id}`).json<Gebruikersgroep>();


const {
  data: listItems,
  isFetching: loadingListItems,
  error: listItemstError
} = useFetchApi(`/waardelijsten`).json<WaardelijstItem[]>();

const waardelijstenModel = ref<Array<string>>([]);

watchEffect(async () => {
  waardelijstenModel.value = gebruikersgroep?.value?.gekoppeldeWaardelijsten || [];
})






// Grouped items
const groupedItems = computed<GroupedWaardeLijstItems>(() =>
  Object.keys(WAARDELIJSTEN).reduce((result: GroupedWaardeLijstItems, key) => {
    result[key as Waardelijst] =
      listItems.value
        ?.filter((item) => item.type === key)
        ?.sort((a, b) => a.name.localeCompare(b.name)) || [];
    return result;
  }, {} as GroupedWaardeLijstItems)
);



// Selected items
// const formData = computed(() => {
//   console.log(gebruikersgroep?.value?.gekoppeldeWaardelijsten)
//   return JSON.stringify(
//     {
//       gekoppeldeWaardelijsten: gebruikersgroep?.value?.gekoppeldeWaardelijsten?.filter((id) =>
//         Object.values(groupedItems.value)
//           .flat()
//           .map((item) => item.id)
//           .includes(id)
//       )
//     }
//   )
// }
// );



const submit = async (): Promise<void> => {


  const formData = JSON.stringify(
    {
      gekoppeldeWaardelijsten: waardelijstenModel?.value?.filter((id) =>
        Object.values(groupedItems.value)
          .flat()
          .map((item) => item.id)
          .includes(id)
      )
    });

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
