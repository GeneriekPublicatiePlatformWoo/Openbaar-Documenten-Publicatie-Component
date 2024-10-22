import { ref, onUnmounted, type ComputedRef } from "vue";

export const useFormValidator = (formRef: ComputedRef<HTMLFormElement | undefined>) => {
  const isValid = ref(false);

  const validateForm = () => {
    if (!formRef.value) {
      isValid.value = false;
      return;
    }

    isValid.value = validateCheckboxGroups(formRef.value) && formRef.value.checkValidity();

    if (!isValid.value) {
      focusInvalidInput(formRef.value);
    }
  };

  const checkboxListeners: (() => void)[] = [];

  const validateCheckboxGroups = (form: HTMLFormElement) => {
    checkboxListeners.forEach((remove) => remove());

    const checkboxGroups = form.querySelectorAll(
      "details[data-required-group='checkbox']"
    ) as NodeListOf<HTMLDetailsElement>;

    if (checkboxGroups.length === 0) return true;

    return Array.from(checkboxGroups).every((group) => {
      group.classList.remove("invalid");

      const checkboxes = group.querySelectorAll(
        '[type="checkbox"]'
      ) as NodeListOf<HTMLInputElement>;

      const isAnyChecked = () => Array.from(checkboxes).some((checkbox) => checkbox.checked);
      
      const onCheckboxChange = () =>
        !isAnyChecked() ? group.classList.add("invalid") : group.classList.remove("invalid");

      // Add listener to reevaluate invalid style
      checkboxes.forEach((checkbox) => {
        checkbox.addEventListener("change", onCheckboxChange);
        checkboxListeners.push(() => checkbox.removeEventListener("change", onCheckboxChange));
      });

      if (!isAnyChecked()) {
        group.classList.add("invalid");
        group.open = true;

        checkboxes[0]?.focus();

        return false;
      }

      return true;
    });
  };

  const focusInvalidInput = (form: HTMLFormElement) => {
    const invalidInputs = form.querySelectorAll("input:invalid") as NodeListOf<HTMLInputElement>;

    invalidInputs.forEach((input) => {
      input.classList.add("invalid");

      const details = input.closest("details");
      if (details) details.open = true;
    });

    invalidInputs[0]?.focus();
  };

  onUnmounted(() => checkboxListeners.forEach((remove) => remove()));

  return {
    validateForm,
    isValid
  };
};
