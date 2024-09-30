<template>
  <prompt-modal :dialog="dialog">
    <p>Weet u zeker dat u dit document wilt verwijderen?</p>
  </prompt-modal>

  <fieldset v-if="model">
    <legend>Documenten</legend>

    <h2>Documenten toevoegen</h2>

    <div v-if="!model.some((doc) => !doc.uuid)" class="form-group">
      <label for="bestand">Selecteer bestanden</label>

      <input
        id="bestand"
        type="file"
        multiple
        title="Selecteer bestanden"
        :accept="accept"
        @change="onFilesSelected"
      />
    </div>

    <details
      v-for="(doc, index) in model.filter((doc) => !doc.uuid)"
      :key="index"
      class="nieuw"
      open
    >
      <summary @click.prevent tabindex="-1">{{ doc.bestandsnaam }}</summary>

      <div class="form-group">
        <label for="titel">Titel</label>

        <input id="titel" type="text" v-model="doc.officieleTitel" required aria-required="true" />
      </div>

      <div class="form-group">
        <label for="verkorte_titel">Verkorte titel</label>

        <input id="verkorte_titel" type="text" v-model="doc.verkorteTitel" />
      </div>

      <div class="form-group">
        <label for="omschrijving">Omschrijving</label>

        <textarea id="omschrijving" v-model="doc.omschrijving" rows="4"></textarea>
      </div>

      <button @click.prevent="onRemoveDocument(index)" class="button secondary icon-after trash">
        Document verwijderen
      </button>
    </details>

    <h2>Toegevoegde documenten</h2>

    <details
      v-for="(doc, index) in model.filter((doc) => doc.uuid)"
      :key="index"
      :class="{ ingetrokken: doc.status === 'ingetrokken' }"
    >
      <summary>{{ doc.officieleTitel }}</summary>

      <dl>
        <dt>Verkorte titel</dt>
        <dd>{{ doc.verkorteTitel || "-" }}</dd>

        <dt>Bestandsnaam</dt>
        <dd>{{ doc.bestandsnaam }}</dd>

        <dt>Omschrijving</dt>
        <dd>{{ doc.omschrijving || "-" }}</dd>
      </dl>

      <button
        v-if="doc.uuid"
        @click.prevent="$emit('toggleDocument', doc.uuid)"
        class="button icon-after note"
        :class="{ secondary: doc.status === 'gepubliceerd' }"
      >
        Document {{ doc.status === "gepubliceerd" ? "intrekken" : "publiceren" }}
      </button>
    </details>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useConfirmDialog } from "@vueuse/core";
import toast from "@/stores/toast";
import PromptModal from "@/components/PromptModal.vue";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap } from "../service";

const props = defineProps<{ documenten: PublicatieDocument[] }>();

const emit = defineEmits<{
  (e: "update:documenten", payload: PublicatieDocument[]): void;
  (e: "update:files", payload: FileList): void;
  (e: "removeDocument", payload: number): void;
  (e: "toggleDocument", payload: string): void;
}>();

const model = computed({
  get: () => props.documenten,
  set: (value) => emit("update:documenten", value)
});

const dialog = useConfirmDialog();

const accept = computed(() => Array.from(mimeTypesMap.value?.keys() || []).join(","));

const onFilesSelected = (event: Event) => {
  const target = event.target as HTMLInputElement;

  if (target.files === null) return;

  const unknownType = Array.from(target.files).some((file) => !mimeTypesMap.value?.get(file.type));

  if (unknownType) {
    target.value = "";

    toast.add({
      text: "Onbekend bestandsformaat.",
      type: "error"
    });

    return;
  }

  emit("update:files", target.files);
};

const onRemoveDocument = async (index: number) => {
  const { isCanceled } = await dialog.reveal();

  if (isCanceled) return;

  emit("removeDocument", index);
};
</script>

<style lang="scss" scoped>
h2 {
  margin-block: 0 var(--spacing-default);
}

button {
  display: flex;
  column-gap: var(--spacing-small);
}

details {
  &.nieuw {
    summary {
      list-style: none;
      pointer-events: none;

      &::-webkit-details-marker {
        display: none;
      }
    }
  }

  &.ingetrokken {
    background-color: #eee;
  }
}

dl {
  margin-block: 0 var(--spacing-default);

  dt {
    color: var(--text);
    font-weight: var(--font-bold);
  }

  dd {
    margin-inline-start: 0;
    margin-block-end: var(--spacing-small);
  }
}
</style>
