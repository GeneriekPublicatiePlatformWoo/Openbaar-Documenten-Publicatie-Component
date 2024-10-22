export default class FormValidator {
  private form: HTMLFormElement;

  constructor(form: HTMLFormElement) {
    this.form = form;
  }

  public isValid(): boolean {
    const isFormValid = this.validateCheckboxGroups() && this.validateNative();

    if (isFormValid) {
      return true;
    } else {
      this.focusInvalidInput();

      return false;
    }
  }

  private validateCheckboxGroups(): boolean {
    const checkboxGroups = this.form.querySelectorAll(
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
  }

  private validateNative(): boolean {
    return this.form.checkValidity();
  }

  private focusInvalidInput(): void {
    const invalidInputs = this.form.querySelectorAll(
      "input:invalid"
    ) as NodeListOf<HTMLInputElement>;

    invalidInputs.forEach((input) => {
      input.classList.add("invalid");

      const details = input.closest("details");
      if (details) details.open = true;
    });

    invalidInputs[0]?.focus();
  }
}
