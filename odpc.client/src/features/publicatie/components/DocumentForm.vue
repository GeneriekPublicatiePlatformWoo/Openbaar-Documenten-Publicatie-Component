<template>
  <fieldset v-if="model">
    <legend>Document</legend>

    <ul class="reset" v-for="(doc, index) in model" :key="index">
      <li v-if="doc.uuid">
        <dl>
          <dt>Titel</dt>
          <dd>{{ doc.officieleTitel }}</dd>

          <dt>Verkorte titel</dt>
          <dd>{{ doc.verkorteTitel }}</dd>

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

      <li v-else>
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
      </li>
    </ul>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap } from "../service";

const props = defineProps<{ documenten: PublicatieDocument[] }>();

const emit = defineEmits<{
  (e: "update:documenten", payload: PublicatieDocument[]): void;
  (e: "update:files", payload: File[]): void;
  (e: "removeDocument", payload: string): void;
}>();

const model = computed({
  get: () => props.documenten,
  set: (value) => emit("update:documenten", value)
});

const accept = computed(() => Array.from(mimeTypesMap.value?.keys() || []).join(","));

const onFileSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;

  if (target.files === null) return;

  const file = target.files[0];

  const bestandsformaat = mimeTypesMap.value?.get(file.type)?.identifier;

  if (!model.value || !bestandsformaat) {
    target.value = "";
    return;
  }

  model.value[0].bestandsnaam = file.name;
  model.value[0].bestandsomvang = file.size;
  model.value[0].bestandsformaat = bestandsformaat;

  emit("update:files", [file]);
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
