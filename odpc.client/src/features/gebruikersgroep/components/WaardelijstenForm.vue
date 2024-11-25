<template>
  <simple-spinner v-if="loading" />

  <alert-inline v-else-if="error"
    >Er is iets misgegaan bij het ophalen van de waardelijsten...</alert-inline
  >

  <fieldset v-else>
    <legend>Waardelijsten</legend>

    <option-group
      v-if="organisaties.length"
      :title="WAARDELIJSTEN.ORGANISATIE"
      :options="organisaties"
      v-model="model"
    />

    <option-group
      v-if="informatiecategorieen.length"
      :title="WAARDELIJSTEN.INFORMATIECATEGORIE"
      :options="informatiecategorieen"
      v-model="model"
    />
  </fieldset>
</template>

<script setup lang="ts">
import { computed, useModel, watch } from "vue";
import OptionGroup from "@/components/option-group/OptionGroup.vue";
import AlertInline from "@/components/AlertInline.vue";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import { WAARDELIJSTEN, type WaardelijstItem } from "../types";
import { useAllPages } from "@/composables/use-all-pages";

const props = defineProps<{ modelValue: string[] }>();

const model = useModel(props, "modelValue");

const {
  data: organisaties,
  loading: organisatiesLoading,
  error: organisatiesError
} = useAllPages<WaardelijstItem>("/api/v1/organisaties");

const {
  data: informatiecategorieen,
  error: informatiecategorieenError,
  loading: informatiecategorieenLoading
} = useAllPages<WaardelijstItem>("/api/v1/informatiecategorieen");

const error = computed(
  () =>
    organisatiesError.value ||
    !organisaties.value.length ||
    informatiecategorieenError.value ||
    !informatiecategorieen.value.length
);

const loading = computed(() => informatiecategorieenLoading.value || organisatiesLoading.value);

const loaded = computed(
  () => !!model.value.length && !!organisaties.value.length && !!informatiecategorieen.value.length
);

const waardelijstUuids = computed(() => [
  ...organisaties.value.map((item) => item.uuid),
  ...informatiecategorieen.value.map((item) => item.uuid)
]);

// Remove uuids from model that are not present/active anymore in ODRC
watch(loaded, (value) => {
  if (value) {
    model.value = model.value.filter((uuid: string) => waardelijstUuids.value.includes(uuid));
  }
});
</script>
