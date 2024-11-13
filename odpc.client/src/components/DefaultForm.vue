<template>
  <form ref="formRef" @invalid.capture="onInvalid">
    <slot></slot>
  </form>
</template>

<script lang="ts" setup>
import { ref } from "vue";

const formRef = ref<HTMLFormElement>();

const onInvalid = () => {
  const invalidInputs = (formRef.value?.querySelectorAll(":user-invalid") ||
    []) as NodeListOf<HTMLInputElement>;

  invalidInputs.forEach((input) => {
    const details = input.closest("details");
    details && (details.open = true);
  });
};
</script>

<style lang="scss" scoped></style>
