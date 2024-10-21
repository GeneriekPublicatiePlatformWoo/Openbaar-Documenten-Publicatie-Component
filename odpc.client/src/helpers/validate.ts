export const validateForm = (form?: HTMLFormElement): { valid?: boolean; invalid?: boolean } => {
  let isFormValid = true;

  // Checkboxes
  const checkboxGroups = form?.querySelectorAll("details[data-required-group='checkbox']") as NodeListOf<HTMLDetailsElement>;

  checkboxGroups.forEach((group) => {
    const checkboxes = group.querySelectorAll('[type="checkbox"]') as NodeListOf<HTMLInputElement>;
    const isAnyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked);

    if (!isAnyChecked) {
      group.classList.add("invalid");
      group.open = true;

      checkboxes[0].focus();

      isFormValid = false;
    } else {
      group.classList.remove("invalid");
    }
  });

  // Invalid individual inputs (native check)
  if (form?.checkValidity() && isFormValid) return { valid: true };

  const inputs = form?.querySelectorAll("input:invalid") as NodeListOf<HTMLInputElement>;

  inputs.forEach((input) => {
    input.classList.add("invalid");

    const details = input.closest("details");
    details && (details.open = true);
  });

  inputs?.[0]?.focus();

  return { invalid: true };
};

