<template>
  <fieldset>
    <legend>Document</legend>

    <div class="form-group">
      <label for="titel">Titel</label>

      <input
        id="titel"
        type="text"
        v-model="model.officieleTitel"
        required
        aria-required="true"
      />
    </div>

    <div class="form-group">
      <label for="verkorte_titel">Verkorte titel</label>

      <input id="verkorte_titel" type="text" v-model="model.verkorteTitel" />
    </div>

    <div class="form-group">
      <label for="omschrijving">Omschrijving</label>

      <textarea id="omschrijving" v-model="model.omschrijving" rows="4"></textarea>
    </div>

    <div class="form-group">
      <label for="bestand">Bestand toevoegen</label>

      <input id="bestand" type="file" />
    </div>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { PublicatieDocument } from "../types";

const props = defineProps<{
  modelValue: PublicatieDocument;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", payload: PublicatieDocument): void;
}>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});
</script>

<style lang="scss" scoped></style>
