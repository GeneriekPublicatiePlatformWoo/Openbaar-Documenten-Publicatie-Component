<template>
  <fieldset>
    <legend>{{ title }}</legend>
    <div class="form-field check-all">
      <label
        ><input type="checkbox" @click="toggleAll" :checked="allSelected" /> selecteer alles
      </label>
    </div>

    <div class="form-field" v-for="({ id, name }, key) in options" :key="key">
      <label><input type="checkbox" :value="id" v-model="model" /> {{ name }} </label>
    </div>
  </fieldset>
</template>

<script setup lang="ts" generic="T extends OptionProps">
import { computed } from "vue";

export type OptionProps = {
  id: string;
  name: string;
};

const props = defineProps<{
  title: string;
  options: T[];
  modelValue: string[];
}>();

const emit = defineEmits<{
  (e: "update:modelValue", payload: string[]): void;
}>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});

const itemIds = computed(() => props.options.map((option) => option.id));

const allSelected = computed(
  () => model.value?.filter((id) => itemIds.value.includes(id)).length === itemIds.value?.length
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
