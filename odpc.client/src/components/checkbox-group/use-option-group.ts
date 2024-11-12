import { ref, watchEffect } from "vue";

export const useOptionGroup = () => {
  const groupRef = ref<HTMLElement>();

  const isAnyChecked = (options: NodeListOf<HTMLInputElement>) =>
    Array.from(options).some((option) => option.checked);

  const setCustomValidity = () => {
    const options = (groupRef.value?.querySelectorAll("[type='checkbox'], [type='radio']") ||
      []) as NodeListOf<HTMLInputElement>;

    options.forEach((option) =>
      option.setCustomValidity(!isAnyChecked(options) ? "Kies minimaal één optie." : "")
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
