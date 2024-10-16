<template>
  <fieldset :disabled="disabled">
    <legend>Publicatie</legend>

    <section>
      <div v-if="!disabled" class="form-group form-group-radio">
        <label>
          <input type="radio" v-model="model.status" :value="PublicatieStatus.gepubliceerd" />
          Gepubliceerd
        </label>

        <label
          ><input type="radio" v-model="model.status" :value="PublicatieStatus.ingetrokken" />
          Ingetrokken</label
        >
      </div>

      <alert-inline v-else>Deze publicatie is ingetrokken.</alert-inline>

      <div class="form-group">
        <label for="uuid">ID</label>

        <input id="uuid" type="text" v-model="model.uuid" readonly aria-readonly="true" />
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
import AlertInline from "@/components/AlertInline.vue";
import { PublicatieStatus, type Publicatie } from "../types";

const props = defineProps<{ modelValue: Publicatie; disabled: boolean }>();

const emit = defineEmits<{ (e: "update:modelValue", payload: Publicatie): void }>();

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit("update:modelValue", value)
});
</script>

<style lang="scss" scoped>
input[type="text"]:read-only {
  background-color: var(--disabled);
}
</style>
