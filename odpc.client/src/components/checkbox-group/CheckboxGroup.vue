<template>
  <details
    role="group"
    ref="groupRef"
    :aria-labelledby="`label-${instanceId}`"
    :aria-required="required ? true : undefined"
    @change="required && setCustomValidity()"
    @invalid.capture="required && onInvalid()"
  >
    <summary :id="`label-${instanceId}`">{{ title }} {{ required ? "*" : "" }}</summary>

    <p v-if="required" class="error" :id="`description-${instanceId}`">Kies minimaal één optie.</p>

    <div class="checkbox check-all">
      <label
        ><input
          type="checkbox"
          @click="toggleAll"
          :checked="allSelected"
          :aria-describedby="`description-${instanceId}`"
          :aria-invalid="!model.length ? true : undefined"
        />
        selecteer alles
      </label>
    </div>

    <div class="checkbox" v-for="({ uuid, naam }, key) in options" :key="key">
      <label
        ><input
          type="checkbox"
          :value="uuid"
          v-model="model"
          :aria-describedby="`description-${instanceId}`"
          :aria-invalid="!model.length ? true : undefined"
        />{{ naam }}</label
      >
    </div>
  </details>
</template>

<script setup lang="ts">
import { computed, getCurrentInstance } from "vue";
import { useCheckboxGroup } from "./use-checkbox-group";
import type { OptionProps } from "./types";

const instanceId = getCurrentInstance()?.uid;

const { groupRef, setCustomValidity, onInvalid } = useCheckboxGroup();

const props = defineProps<{
  title: string;
  options: OptionProps[];
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

const itemIds = computed(() => props.options.map((option) => option.uuid));

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

details {
  &:has(:user-invalid) {
    border-color: var(--code);

    .error {
      color: var(--code);
    }
  }
}
</style>
