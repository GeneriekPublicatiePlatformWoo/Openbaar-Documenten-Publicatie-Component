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
import { ref, watch, useModel } from "vue";

const props = defineProps<{ fromDate: string; untilDate: string }>();

const fromDate = useModel(props, "fromDate");
const untilDate = useModel(props, "untilDate");

const fromDateRef = ref<HTMLInputElement>();
const untilDateRef = ref<HTMLInputElement>();

const today = ref(new Date().toISOString().split("T")[0]);

watch(fromDate, (value) => {
  if (!untilDateRef.value) return;

  if (value) {
    if (value > today.value) {
      fromDate.value = "";
      return;
    }

    untilDateRef.value.min = value;

    if (untilDate.value && untilDate.value < value) {
      untilDate.value = value;
    }
  } else {
    untilDateRef.value.removeAttribute("min");
  }
});

watch(untilDate, (value) => {
  if (!fromDateRef.value) return;

  if (value) {
    if (value > today.value) {
      untilDate.value = "";
      return;
    }

    fromDateRef.value.max = value;

    if (fromDate.value && value < fromDate.value) {
      fromDate.value = value;
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
