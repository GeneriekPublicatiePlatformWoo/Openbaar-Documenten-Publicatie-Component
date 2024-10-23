import { ref, watchEffect, onUnmounted } from "vue";

export const useCheckboxGroup = () => {
  const groupRef = ref<HTMLElement>();

  const isAnyChecked = (checkboxes: NodeListOf<HTMLInputElement>) =>
    Array.from(checkboxes).some((checkbox) => checkbox.checked);

  const setCustomValidityCheckboxGroup = () => {
    const checkboxes = (groupRef.value?.querySelectorAll("[type='checkbox']") ||
      []) as NodeListOf<HTMLInputElement>;

    checkboxes.forEach((checkbox) =>
      checkbox.setCustomValidity(!isAnyChecked(checkboxes) ? "Kies minimaal één optie." : "")
    );
  };

  const invalidHandler = () =>
    groupRef.value instanceof HTMLDetailsElement && (groupRef.value.open = true);

  const addListeners = () => {
    if (!groupRef.value || !groupRef.value.hasAttribute("aria-required")) return;

    setCustomValidityCheckboxGroup();

    groupRef.value.addEventListener("change", setCustomValidityCheckboxGroup);
    groupRef.value.addEventListener("invalid", invalidHandler, true);
  };

  const removeListeners = () => {
    groupRef.value?.removeEventListener("change", setCustomValidityCheckboxGroup);
    groupRef.value?.removeEventListener("invalid", invalidHandler, true);
  };

  watchEffect(() => {
    removeListeners();
    addListeners();
  });

  onUnmounted(() => removeListeners());

  return {
    groupRef
  };
};
