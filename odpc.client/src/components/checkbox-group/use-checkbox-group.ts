import { ref, watchEffect } from "vue";

export const useCheckboxGroup = () => {
  const groupRef = ref<HTMLElement>();

  const isAnyChecked = (checkboxes: NodeListOf<HTMLInputElement>) =>
    Array.from(checkboxes).some((checkbox) => checkbox.checked);

  const setCustomValidity = () => {
    const checkboxes = (groupRef.value?.querySelectorAll("[type='checkbox']") ||
      []) as NodeListOf<HTMLInputElement>;

    checkboxes.forEach((checkbox) =>
      checkbox.setCustomValidity(!isAnyChecked(checkboxes) ? "Kies minimaal één optie." : "")
    );
  };

  const onInvalid = () =>
    groupRef.value instanceof HTMLDetailsElement && (groupRef.value.open = true);

  watchEffect(() => groupRef.value?.hasAttribute("aria-required") && setCustomValidity());

  return {
    groupRef,
    setCustomValidity,
    onInvalid
  };
};
