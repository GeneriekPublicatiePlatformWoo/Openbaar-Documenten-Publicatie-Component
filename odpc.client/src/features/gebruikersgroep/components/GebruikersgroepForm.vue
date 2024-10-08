<template>
  <fieldset>
    <legend>Gebruikersgroep gegevens</legend>

    <div class="form-group">
      <label for="titel">Naam *</label>

      <input
        id="titel"
        type="text"
        v-model="model.naam"
        required
        aria-required="true"
        aria-describedby="titelError"
        :aria-invalid="!model.naam"
      />

      <span id="titelError" class="error">Naam is een verplicht veld</span>
    </div>

    <div class="form-group">
      <label for="omschrijving">Omschrijving</label>

      <textarea id="omschrijving" v-model="model.omschrijving" rows="4"></textarea>
    </div>

    <div class="form-group form-group-button">
      <label for="gebruiker">Gebruiker toevoegen</label>

      <input
        id="gebruiker"
        type="text"
        v-model="gebruiker"
        :aria-invalid="!gebruiker.trim().length"
        @keydown.enter.prevent="onAddGebruiker"
      />

      <button
        type="button"
        :disabled="!gebruiker.trim().length"
        :aria-disabled="!gebruiker.trim().length"
        @click="onAddGebruiker"
      >
        Toevoegen
      </button>
    </div>

    <details ref="detailsRef" aria-live="polite">
      <summary>Toegevoegde gebruikers</summary>

      <p v-if="!model.gekoppeldeGebruikers.length">Er zijn nog geen gebruikers toegevoegd.</p>

      <ul v-else class="reset">
        <li v-for="(gekoppeldeGebruiker, index) in model.gekoppeldeGebruikers" :key="index">
          <button
            type="button"
            :title="`${gekoppeldeGebruiker} verwijderen`"
            :aria-label="`${gekoppeldeGebruiker} verwijderen`"
            class="button secondary icon-after xmark"
            @click="$emit('removeGebruiker', index)"
          >
            {{ gekoppeldeGebruiker }}
          </button>
        </li>
      </ul>
    </details>
  </fieldset>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import toast from "@/stores/toast";
import type { Gebruikersgroep } from "../types";

const props = defineProps<{ modelValue: Gebruikersgroep }>();

const emit = defineEmits<{
  (e: "update:modelValue", payload: Gebruikersgroep): void;
  (e: "addGebruiker", payload: string): void;
  (e: "removeGebruiker", payload: number): void;
}>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});

const detailsRef = ref<HTMLDetailsElement>();

const gebruiker = ref("");

const onAddGebruiker = () => {
  if (!gebruiker.value.trim().length) return;

  detailsRef.value && (detailsRef.value.open = true);

  if (model.value.gekoppeldeGebruikers.includes(gebruiker.value)) {
    toast.add({
      text: "Deze gebruiker is al toegevoegd.",
      type: "error"
    });
  } else {
    emit("addGebruiker", gebruiker.value.trim());
  }

  gebruiker.value = "";
};
</script>

<style lang="scss" scoped>
ul {
  display: flex;
  flex-wrap: wrap;
  column-gap: var(--spacing-small);
}

.form-group-button {
  display: grid;
  grid-template-columns: 1fr auto;
  column-gap: var(--spacing-small);

  label {
    grid-column: span 2;
  }
}
</style>
