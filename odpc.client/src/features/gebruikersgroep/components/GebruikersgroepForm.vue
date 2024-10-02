<template>
  <fieldset>
    <legend>Gebruikersgroep</legend>
    
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
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { Gebruikersgroep } from "../types";

const props = defineProps<{ modelValue: Gebruikersgroep }>();

const emit = defineEmits<{ (e: "update:modelValue", payload: Gebruikersgroep): void }>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});
</script>
