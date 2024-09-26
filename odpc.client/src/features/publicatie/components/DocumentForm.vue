<template>
  <fieldset v-if="model">
    <legend>Documenten</legend>

    <ul class="reset">
      <template v-for="(doc, index) in model" :key="index">
        <li v-if="!doc.uuid">
          <h2>{{ doc.bestandsnaam }}</h2>

          <div class="form-group">
            <label for="titel">Titel</label>

            <input
              id="titel"
              type="text"
              v-model="doc.officieleTitel"
              required
              aria-required="true"
            />
          </div>

          <div class="form-group">
            <label for="verkorte_titel">Verkorte titel</label>

            <input id="verkorte_titel" type="text" v-model="doc.verkorteTitel" />
          </div>

          <div class="form-group">
            <label for="omschrijving">Omschrijving</label>

            <textarea id="omschrijving" v-model="doc.omschrijving" rows="4"></textarea>
          </div>
        </li>

        <li v-else-if="doc.status === 'gepubliceerd'">
          <dl>
            <dt>Titel</dt>
            <dd>{{ doc.officieleTitel }}</dd>

            <dt>Verkorte titel</dt>
            <dd>{{ doc.verkorteTitel || "-" }}</dd>

            <dt>Omschrijving</dt>
            <dd>{{ doc.omschrijving || "-" }}</dd>

            <dt>Bestandsnaam</dt>
            <dd>{{ doc.bestandsnaam }}</dd>
          </dl>

          <button
            @click.prevent="$emit('removeDocument', doc.uuid)"
            class="button secondary icon-after trash"
          >
            Verwijder document
          </button>
        </li>
      </template>
    </ul>

    <div class="form-group">
      <label for="bestand">Bestanden toevoegen</label>

      <input
        id="bestand"
        type="file"
        multiple
        title="Voeg bestand toe"
        :accept="accept"
        @change="onFilesSelected"
      />
    </div>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap } from "../service";

const props = defineProps<{ documenten: PublicatieDocument[] }>();

const emit = defineEmits<{
  (e: "update:documenten", payload: PublicatieDocument[]): void;
  (e: "update:files", payload: FileList): void;
  (e: "removeDocument", payload: string): void;
}>();

const model = computed({
  get: () => props.documenten,
  set: (value) => emit("update:documenten", value)
});

const accept = computed(() => Array.from(mimeTypesMap.value?.keys() || []).join(","));

const onFilesSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;

  if (target.files === null) return;

  // if (!model.value || !bestandsformaat) {
  //   target.value = "";
  //   return;
  // }

  emit("update:files", target.files);
};
</script>

<style lang="scss" scoped>
section h2:first-child { // ...
  font-size: inherit;
  margin-top: 0;
}

button {
  display: flex;
  column-gap: var(--spacing-small);
}

ul {
  display: flex;
  flex-direction: column;
  gap: var(--spacing-default);
  margin-block-end: var(--spacing-large);

  li {
    border: 1px solid var(--border);
    padding: 1rem;
    border-radius: var(--standard-border-radius);
  }
}

dl {
  margin-block: 0 var(--spacing-default);

  dt {
    font-weight: var(--font-bold);
  }

  dd {
    margin-inline-start: 0;
    margin-block-end: var(--spacing-small);
  }
}
</style>
