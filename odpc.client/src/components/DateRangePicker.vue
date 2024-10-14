<template>
  <fieldset>
    <div class="form-group">
      <label for="fromDate">Van</label>

      <input
        type="date"
        id="fromDate"
        ref="fromDateRef"
        v-model="fromDateModel"
        @change="onFromDateChange"
        :max="today"
      />
    </div>

    <div class="form-group">
      <label for="untilDate">Tot</label>

      <input
        type="date"
        id="untilDate"
        ref="untilDateRef"
        v-model="untilDateModel"
        @change="onUntilDateChange"
        :max="today"
      />
    </div>
  </fieldset>
</template>

<script setup lang="ts">
import { ref, watch, watchEffect } from "vue";

const props = defineProps<{ fromDate: string; untilDate: string }>();

const emit = defineEmits<{
  (e: "update:fromDate", payload: string): void;
  (e: "update:untilDate", payload: string): void;
}>();

const today = ref(new Date().toISOString().split("T")[0]);

const fromDateModel = ref<string>("");
const untilDateModel = ref<string>("");

watchEffect(() => {
  fromDateModel.value = props.fromDate;
  untilDateModel.value = props.untilDate;
});

const onFromDateChange = () => emit("update:fromDate", fromDateModel.value);
const onUntilDateChange = () => emit("update:untilDate", untilDateModel.value);

const fromDateRef = ref<HTMLInputElement>();
const untilDateRef = ref<HTMLInputElement>();

watch(fromDateModel, (value) => {
  if (!fromDateRef.value || !untilDateRef.value) return;

  if (value) {
    if (value > today.value) {
      fromDateRef.value.value = "";
      fromDateRef.value.dispatchEvent(new Event("input"));
      return;
    }

    untilDateRef.value.min = value;

    if (untilDateModel.value && untilDateModel.value < value) {
      untilDateRef.value.value = value;
      untilDateRef.value.dispatchEvent(new Event("input"));
    }
  } else {
    untilDateRef.value.removeAttribute("min");
  }
});

watch(untilDateModel, (value) => {
  if (!fromDateRef.value || !untilDateRef.value) return;

  if (value) {
    if (value > today.value) {
      untilDateRef.value.value = "";
      untilDateRef.value.dispatchEvent(new Event("input"));
      return;
    }

    fromDateRef.value.max = value;

    if (fromDateModel.value && value < fromDateModel.value) {
      fromDateRef.value.value = value;
      fromDateRef.value.dispatchEvent(new Event("input"));
    }
  } else {
    fromDateRef.value.max = today.value;
  }
});
</script>

<style lang="scss" scoped>
fieldset {
  display: flex;
  column-gap: var(--spacing-default);

  .form-group {
    flex-grow: 1;
  }
}
</style>
