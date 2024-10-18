<template>
  <fieldset>
    <legend>Waardelijsten</legend>

    <checkbox-list
      v-for="(value, key) in WAARDELIJSTEN"
      :key="key"
      :title="value"
      :options="groupedWaardelijstItems[key]"
      v-model="model"
    />
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import CheckboxList from "@/components/CheckboxList.vue";
import { WAARDELIJSTEN, groupedWaardelijstItems } from "@/features/waardelijst";

const props = defineProps<{ modelValue: string[] }>();

const emit = defineEmits<{ (e: "update:modelValue", payload: string[]): void }>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});
</script>
