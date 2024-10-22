import { onMounted, onUnmounted, ref, type ComputedRef } from "vue";

export const useFormValidator = (formRef: ComputedRef<HTMLFormElement | undefined>) => {
  const isValid = ref<boolean>(false);

  const validateForm = () => {
    if (!formRef.value) {
      isValid.value = false;
      return;
    }

    isValid.value = validateCheckboxGroups(formRef.value) && formRef.value.checkValidity();

    !isValid.value && focusInvalidInput(formRef.value);
  };

  const validateCheckboxGroups = (form: HTMLFormElement) => {
    const checkboxGroups = form.querySelectorAll(
      "details[data-required-group='checkbox']"
    ) as NodeListOf<HTMLDetailsElement>;

    return Array.from(checkboxGroups).every((group) => {
      const checkboxes = group.querySelectorAll(
        '[type="checkbox"]'
      ) as NodeListOf<HTMLInputElement>;

      const isAnyChecked = () => Array.from(checkboxes).some((checkbox) => checkbox.checked);

      if (!isAnyChecked()) {
        group.classList.add("invalid");
        group.open = true;

        checkboxes[0]?.focus();

        return false;
      } else {
        group.classList.remove("invalid");

        return true;
      }
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

  onMounted(() => console.log(`add listeners`));
  onUnmounted(() => console.log(`remove listeners`));

  return {
    validateForm,
    isValid
  };
};
