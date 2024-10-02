export const validateForm = (
  form: HTMLFormElement | undefined
): { valid?: boolean; invalid?: boolean } => {
  if (form?.checkValidity()) return { valid: true };

  const inputs = form?.querySelectorAll("input:invalid") as NodeListOf<HTMLInputElement>;

  inputs.forEach((input) => {
    input.classList.add("invalid");

    const details = input.closest("details");
    details && (details.open = true);
  });

  inputs?.[0].focus();

  return { invalid: true };
};
