<template>
  <div class="form-group">
    <label for="fromDate">Datum van</label>

    <input type="date" id="fromDate" ref="fromDateRef" v-model="fromDate" :max="today" />
  </div>

  <div class="form-group">
    <label for="untilDate">Datum tot</label>

    <input type="date" id="untilDate" ref="untilDateRef" v-model="untilDate" :max="today" />
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from "vue";

const props = defineProps<{ fromDate: string; untilDate: string }>();

const emit = defineEmits<{
  (e: "update:fromDate", payload: string): void;
  (e: "update:untilDate", payload: string): void;
}>();

const fromDate = computed<string>({
  get: () => props.fromDate,
  set: (value) => emit("update:fromDate", value)
});
const untilDate = computed<string>({
  get: () => props.untilDate,
  set: (value) => emit("update:untilDate", value)
});

const today = ref(new Date().toISOString().split("T")[0]);

const fromDateRef = ref<HTMLInputElement>();
const untilDateRef = ref<HTMLInputElement>();

watch(fromDate, (value) => {
  if (!fromDateRef.value || !untilDateRef.value) return;

  if (value) {
    if (value > today.value) {
      fromDateRef.value.value = "";
      fromDateRef.value.dispatchEvent(new Event("input"));
      return;
    }

    untilDateRef.value.min = value;

    if (untilDate.value && untilDate.value < value) {
      untilDateRef.value.value = value;
      untilDateRef.value.dispatchEvent(new Event("input"));
    }
  } else {
    untilDateRef.value.removeAttribute("min");
  }
});

watch(untilDate, (value) => {
  if (!fromDateRef.value || !untilDateRef.value) return;

  if (value) {
    if (value > today.value) {
      untilDateRef.value.value = "";
      untilDateRef.value.dispatchEvent(new Event("input"));
      return;
    }

    fromDateRef.value.max = value;

    if (fromDate.value && value < fromDate.value) {
      fromDateRef.value.value = value;
      fromDateRef.value.dispatchEvent(new Event("input"));
    }
  } else {
    fromDateRef.value.max = today.value;
  }
});
</script>

<style lang="scss" scoped>
input,
.form-group {
  margin-block-end: 0;
}
</style>
