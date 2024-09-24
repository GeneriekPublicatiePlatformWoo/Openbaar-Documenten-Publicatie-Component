<template>
  <fieldset>
    <legend>Document</legend>

    <section v-if="model">
      <template v-if="model.uuid">
        <dl>
          <dt>Titel</dt>
          <dd>{{ model.officieleTitel }}</dd>

          <dt>Verkorte titel</dt>
          <dd>{{ model.verkorteTitel }}</dd>

          <dt>Omschrijving</dt>
          <dd>{{ model.omschrijving || "-" }}</dd>

          <dt>Bestandsnaam</dt>
          <dd>{{ model.bestandsnaam }}</dd>
        </dl>

        <button @click="$emit('reset')" class="button secondary icon-after trash">
          Verwijder document
        </button>
      </template>

      <template v-else>
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

          <input
            id="bestand"
            type="file"
            title="Voeg bestand toe"
            required
            aria-required="true"
            :accept="accept"
            @change="onFileSelected"
          />
        </div>
      </template>
    </section>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap } from "../service";

const props = defineProps<{ publicatieDocument: PublicatieDocument }>();

const emit = defineEmits<{
  (e: "update:publicatieDocument", payload: PublicatieDocument): void;
  (e: "update:file", payload: File): void;
  (e: "reset"): void;
}>();

const model = computed({
  get: () => props.publicatieDocument,
  set: (value) => emit("update:publicatieDocument", value)
});

const accept = computed(() => Array.from(mimeTypesMap.value?.keys() || []).join(','));

const onFileSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;

  if (target.files === null) return;

  const file = target.files[0];

  const bestandsformaat = mimeTypesMap.value?.get(file.type)?.identifier;

  if (!model.value || !bestandsformaat) {
    target.value = "";
    return;
  }

  model.value.bestandsnaam = file.name;
  model.value.bestandsomvang = file.size;
  model.value.bestandsformaat = bestandsformaat;

  emit("update:file", file);
};
</script>

<style lang="scss" scoped>
button {
  display: flex;
  column-gap: var(--spacing-small);
}

dl {
  margin-block: 0;

  dt {
    font-weight: var(--font-bold);
  }

  dd {
    margin-inline-start: 0;
    margin-block-end: var(--spacing-default);
  }
}
</style>
