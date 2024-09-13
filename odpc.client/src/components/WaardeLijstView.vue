<template>
  <fieldset>
    <legend>{{ title }}</legend>
    <div class="form-field check-all">
      <label><input type="checkbox" @click="toggleAll" :checked="allSelected" /> selecteer alles
      </label>
    </div>

    <div class="form-field" v-for="({ id, name }, key) in items" :key="key">
      <label><input type="checkbox" :value="id" v-model="model" /> {{ name }} </label>
    </div>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { type WaardelijstItem } from "@/../mock/api.mock";

const props = defineProps<{
  title: string;
  items: WaardelijstItem[];
  modelValue: string[] | undefined;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", payload: string[] | undefined): void;
}>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});

const itemIds = computed(() => props.items.map((item) => item.id));

const allSelected = computed(
  () => model.value?.filter((id) => itemIds.value.includes(id)).length === itemIds.value.length
);

const toggleAll = () => {
  model.value = allSelected.value
    ? model.value?.filter((id) => !itemIds.value.includes(id)) || []
    : [...new Set([...(model.value || []), ...itemIds.value])];
};
</script>

<style lang="scss" scoped>
.check-all {
  padding-block-end: var(--spacing-small);
  margin-block-end: var(--spacing-small);
  border-bottom: 1px solid var(--color-grey);
}
</style>
