<template>
  <details data-required-group="checkbox">
    <summary>{{ title }} {{ required ? "*" : "" }}</summary>

    <p v-if="required" class="error" :id="`description-${getCurrentInstance()?.uid}`">
      Er moet minimaal één optie gekozen worden.
    </p>

    <div class="checkbox check-all">
      <label
        ><input type="checkbox" @click="toggleAll" :checked="allSelected" /> selecteer alles
      </label>
    </div>

    <div class="checkbox" v-for="({ id, name }, key) in options" :key="key">
      <label
        ><input
          type="checkbox"
          :value="id"
          v-model="model"
          :aria-describedby="`description-${getCurrentInstance()?.uid}`"
        />
        {{ name }}
      </label>
    </div>
  </details>
</template>

<script setup lang="ts" generic="T extends OptionProps">
import { computed, getCurrentInstance } from "vue";

export type OptionProps = {
  id: string;
  name: string;
};

const props = defineProps<{
  title: string;
  options: T[];
  modelValue: string[];
  required?: boolean;
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
  () =>
    !!itemIds.value?.length &&
    model.value?.filter((id) => itemIds.value.includes(id)).length === itemIds.value?.length
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
