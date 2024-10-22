import { ref, onUnmounted, watchEffect } from "vue";

export const useFormValidator = () => {
  const formRef = ref<HTMLFormElement>();

  const isValid = ref(false);

  const validateForm = () => {
    if (!formRef.value) {
      isValid.value = false;
      return;
    }

    isValid.value = validateCheckboxGroups() && formRef.value.checkValidity();

    if (!isValid.value) {
      focusInvalidInput(formRef.value);
    }
  };

  const validateCheckboxGroups = () => {
    const checkboxGroups = getCheckboxGroups();

    if (checkboxGroups.length === 0) return true;

    return Array.from(checkboxGroups).every((group) => {
      group.classList.remove("invalid");

      const checkboxes = getCheckboxesFromGroup(group);

      if (!isAnyChecked(checkboxes)) {
        group.classList.add("invalid");
        group.open = true;

        checkboxes[0]?.focus();

        return false;
      }

      return true;
    });
  };

  // Input type text
  const focusInvalidInput = (form: HTMLFormElement) => {
    const invalidInputs = form.querySelectorAll(
      "input[type='text']:invalid"
    ) as NodeListOf<HTMLInputElement>;

    invalidInputs.forEach((input) => {
      input.classList.add("invalid");

      const details = input.closest("details");
      if (details) details.open = true;
    });

    invalidInputs[0]?.focus();
  };

  // Input type checkbox
  const getCheckboxGroups = () =>
    formRef.value?.querySelectorAll(
      "details[data-required-group='checkbox']"
    ) as NodeListOf<HTMLDetailsElement>;

  const getCheckboxesFromGroup = (group: HTMLDetailsElement) =>
    group.querySelectorAll("[type='checkbox']") as NodeListOf<HTMLInputElement>;

  const isAnyChecked = (checkboxes: NodeListOf<HTMLInputElement>) =>
    Array.from(checkboxes).some((checkbox) => checkbox.checked);

  const onCheckboxChange = (group: HTMLDetailsElement, checkboxes: NodeListOf<HTMLInputElement>) =>
    !isAnyChecked(checkboxes) ? group.classList.add("invalid") : group.classList.remove("invalid");

  const checkboxListeners: (() => void)[] = [];

  // Add listeners to reevaluate invalid style
  const addCheckboxListeners = () => {
    checkboxListeners.forEach((remove) => remove());

    getCheckboxGroups().forEach((group) => {
      const checkboxes = getCheckboxesFromGroup(group);

      checkboxes.forEach((checkbox) => {
        checkbox.addEventListener("change", () => onCheckboxChange(group, checkboxes));

        checkboxListeners.push(() =>
          checkbox.removeEventListener("change", () => onCheckboxChange(group, checkboxes))
        );
      });
    });
  };

  watchEffect(() => formRef.value && addCheckboxListeners());

  onUnmounted(() => checkboxListeners.forEach((remove) => remove()));

  return {
    formRef,
    isValid,
    validateForm
  };
};
