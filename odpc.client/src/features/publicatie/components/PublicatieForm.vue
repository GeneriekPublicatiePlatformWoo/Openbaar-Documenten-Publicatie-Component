<template>
  <fieldset>
    <legend>Publicatie</legend>

    <section>
      <div class="form-group form-group-radio">
        <label>
          <input type="radio" v-model="model.status" :value="PublicatieStatus.gepubliceerd" />
          Gepubliceerd
        </label>

        <label
          ><input type="radio" v-model="model.status" :value="PublicatieStatus.ingetrokken" />
          Ingetrokken</label
        >
      </div>

      <div class="form-group">
        <label for="uuid">ID</label>

        <input id="uuid" type="text" v-model="model.uuid" readonly />
      </div>

      <div class="form-group">
        <label for="titel">Titel *</label>

        <input
          id="titel"
          type="text"
          v-model="model.officieleTitel"
          required
          aria-required="true"
          aria-describedby="titelError"
          :aria-invalid="!model.officieleTitel"
        />

        <span id="titelError" class="error">Titel is een verplicht veld</span>
      </div>

      <div class="form-group">
        <label for="verkorte_titel">Verkorte titel</label>

        <input id="verkorte_titel" type="text" v-model="model.verkorteTitel" />
      </div>

      <div class="form-group">
        <label for="omschrijving">Omschrijving</label>

        <textarea id="omschrijving" v-model="model.omschrijving" rows="4"></textarea>
      </div>
    </section>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { PublicatieStatus, type Publicatie } from "../types";

const props = defineProps<{ modelValue: Publicatie }>();

const emit = defineEmits<{ (e: "update:modelValue", payload: Publicatie): void }>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});
</script>

<style lang="scss" scoped>
input:read-only {
  background-color: var(--accent-bg);
}
</style>
