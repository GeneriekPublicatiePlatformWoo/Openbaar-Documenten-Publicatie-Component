<template>
  <fieldset>
    <legend>Waardelijsten</legend>

    <checkbox-group
      v-if="organisaties.length"
      :title="WAARDELIJSTEN.ORGANISATIE"
      :options="organisaties"
      v-model="model"
    />

    <checkbox-group
      v-if="informatiecategorieen.length"
      :title="WAARDELIJSTEN.INFORMATIECATEGORIE"
      :options="informatiecategorieen"
      v-model="model"
    />
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import CheckboxGroup from "@/components/checkbox-group/CheckboxGroup.vue";
import { WAARDELIJSTEN, organisaties, informatiecategorieen } from "../";

const props = defineProps<{ modelValue: string[] }>();

const emit = defineEmits<{ (e: "update:modelValue", payload: string[]): void }>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});
</script>
