import { computed, ref, watchEffect } from "vue";

export const useOptionGroup = () => {
  const groupRef = ref<HTMLElement>();

  const required = computed(() => groupRef.value?.hasAttribute("aria-required"));

  const getMessage = (type: string) =>
    type === "radio" ? "Kies één optie." : "Kies minimaal één optie.";

  const isAnyChecked = (options: NodeListOf<HTMLInputElement>) =>
    Array.from(options).some((option) => option.checked);

  const setCustomValidity = () => {
    if (!required.value) return;

    const options = (groupRef.value?.querySelectorAll("[type='checkbox'], [type='radio']") ||
      []) as NodeListOf<HTMLInputElement>;

    options.forEach((option) =>
      option.setCustomValidity(!isAnyChecked(options) ? getMessage(option.type) : "")
    );
  };

  watchEffect(() => setCustomValidity());

  return {
    groupRef,
    setCustomValidity,
    getMessage
  };
};
