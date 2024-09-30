<template>
  <dialog ref="dialogRef" @close="onClose">
    <form method="dialog">
      <slot></slot>

      <menu class="reset">
        <li>
          <button type="submit" value="cancel" class="button secondary">
            {{ cancelMessage }}
          </button>
        </li>

        <li>
          <button type="submit" value="confirm" v-focus>
            {{ confirmMessage }}
          </button>
        </li>
      </menu>
    </form>
  </dialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { whenever, type UseConfirmDialogReturn } from "@vueuse/core";

const props = withDefaults(
  defineProps<{
    dialog: UseConfirmDialogReturn<unknown, unknown, unknown>;
    message: string;
    cancelMessage: string;
    confirmMessage: string;
  }>(),
  {
    message: "",
    cancelMessage: "Nee",
    confirmMessage: "Ja"
  }
);

const dialogRef = ref<HTMLDialogElement>();

const onClose = () => {
  if (dialogRef.value?.returnValue === "confirm") {
    props.dialog.confirm();
  } else {
    props.dialog.cancel();
  }
};

whenever(
  () => props.dialog.isRevealed.value,
  () => {
    dialogRef.value?.showModal();
  },
  { immediate: true }
);
</script>

<style lang="scss" scoped>
menu {
  display: flex;
  gap: var(--spacing-default);
  justify-content: flex-end;
}

form {
  display: flex;
  flex-direction: column;
  gap: var(--spacing-default);
}

dialog {
  min-width: 33%;
  padding: var(--spacing-large);
  border: 1px solid var(--border);
  border-radius: var(--radius-default);
}

::backdrop {
  background-color: rgb(102 102 102 / 80%);
}
</style>
