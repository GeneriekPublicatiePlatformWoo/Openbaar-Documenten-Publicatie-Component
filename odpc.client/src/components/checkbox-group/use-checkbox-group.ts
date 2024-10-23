import { ref, onUnmounted, watchEffect } from "vue";

export const useCheckboxGroup = () => {
  const groupRef = ref<HTMLElement>();

  const isAnyChecked = (checkboxes: NodeListOf<HTMLInputElement>) =>
    Array.from(checkboxes).some((checkbox) => checkbox.checked);

  const onCheckboxChange = (checkboxes: NodeListOf<HTMLInputElement>) =>
    !isAnyChecked(checkboxes)
      ? setCustomValidityCheckboxGroup(checkboxes)
      : clearCustomValidityCheckboxGroup(checkboxes);

  const setCustomValidityCheckboxGroup = (checkboxes: NodeListOf<HTMLInputElement>) =>
    checkboxes.forEach((checkbox) => checkbox.setCustomValidity("Kies minimaal één optie."));

  const clearCustomValidityCheckboxGroup = (checkboxes: NodeListOf<HTMLInputElement>) =>
    checkboxes.forEach((checkbox) => checkbox.setCustomValidity(""));

  const checkboxListeners: (() => void)[] = [];

  const removeCheckboxListeners = () => checkboxListeners.forEach((remove) => remove());

  const addCheckboxListeners = () => {
    removeCheckboxListeners();

    if (!groupRef.value || !groupRef.value.hasAttribute("aria-required")) return;

    const checkboxes = groupRef.value.querySelectorAll(
      "[type='checkbox']"
    ) as NodeListOf<HTMLInputElement>;

    if (!checkboxes?.length) return;

    if (!isAnyChecked(checkboxes)) setCustomValidityCheckboxGroup(checkboxes);

    const invalidHandler = () =>
      groupRef.value instanceof HTMLDetailsElement && (groupRef.value.open = true);

    const changeHandler = () => onCheckboxChange(checkboxes);

    checkboxes.forEach((checkbox) => {
      checkbox.addEventListener("invalid", invalidHandler);
      checkboxListeners.push(() => checkbox.removeEventListener("invalid", invalidHandler));

      checkbox.addEventListener("change", changeHandler);
      checkboxListeners.push(() => checkbox.removeEventListener("change", changeHandler));
    });
  };

  watchEffect(() => groupRef.value && addCheckboxListeners());

  onUnmounted(() => removeCheckboxListeners());

  return {
    groupRef
  };
};
