<template>
  <fieldset v-if="model" aria-live="polite">
    <legend>Documenten</legend>

    <template v-if="!model.some((doc) => !doc.uuid)">
      <file-upload @filesSelected="filesSelected" />

      <h2 v-if="model.length">Toegevoegde documenten</h2>
    </template>

    <details
      v-for="(doc, index) in model"
      :key="index"
      :class="{ nieuw: !doc.uuid, ingetrokken: doc.status === 'ingetrokken' }"
      :open="!doc.uuid"
    >
      <template v-if="doc.uuid">
        <summary>{{ doc.bestandsnaam }}</summary>

        <div class="form-group form-group-radio">
          <label>
            <input type="radio" v-model="doc.status" value="gepubliceerd" /> Gepubliceerd
          </label>

          <label><input type="radio" v-model="doc.status" value="ingetrokken" /> Ingetrokken</label>
        </div>
      </template>

      <summary v-else @click.prevent tabindex="-1">
        Nieuw: <span>{{ doc.bestandsnaam }}</span>
      </summary>

      <div class="form-group">
        <label for="titel">Titel *</label>

        <input
          id="titel"
          type="text"
          v-model="doc.officieleTitel"
          required
          aria-required="true"
          :aria-describedby="`titelError-${index}`"
          :aria-invalid="!doc.officieleTitel"
        />

        <span :id="`titelError-${index}`" class="error">Titel is een verplicht veld</span>
      </div>

      <div class="form-group">
        <label for="verkorte_titel">Verkorte titel</label>

        <input id="verkorte_titel" type="text" v-model="doc.verkorteTitel" />
      </div>

      <div class="form-group">
        <label for="omschrijving">Omschrijving</label>

        <textarea id="omschrijving" v-model="doc.omschrijving" rows="4"></textarea>
      </div>

      <button
        v-if="!doc.uuid"
        type="button"
        class="button secondary icon-after trash"
        @click="onRemoveDocument(index)"
      >
        Verwijderen
      </button>
    </details>

    <prompt-modal :dialog="dialog" confirm-message="Ja, verwijderen" cancel-message="Nee, behouden">
      <p>Weet u zeker dat u dit document wilt verwijderen?</p>
    </prompt-modal>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useConfirmDialog } from "@vueuse/core";
import toast from "@/stores/toast";
import PromptModal from "@/components/PromptModal.vue";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap } from "../service";
import FileUpload from "./FileUpload.vue";

const props = defineProps<{ documenten: PublicatieDocument[] }>();

const emit = defineEmits<{
  (e: "update:documenten", payload: PublicatieDocument[]): void;
  (e: "update:files", payload: File[]): void;
  (e: "removeDocument", payload: number): void;
  (e: "toggleDocument", payload: string): void;
}>();

const model = computed<PublicatieDocument[]>({
  get: () => props.documenten,
  set: (value) => emit("update:documenten", value)
});

const dialog = useConfirmDialog();

const filesSelected = (event: Event | DragEvent) => {
  const files: File[] =
    event instanceof DragEvent
      ? [...(event.dataTransfer?.files || [])]
      : [...((event.target as HTMLInputElement).files || [])];

  const unknownType = files.some((file) => !mimeTypesMap.value?.get(file.type));

  if (!files.length || unknownType) {
    event instanceof DragEvent
      ? event.dataTransfer?.clearData()
      : ((event.target as HTMLInputElement).value = "");

    toast.add({
      text: "Onbekend bestandsformaat.",
      type: "error"
    });

    return;
  }

  emit("update:files", files);
};

const onRemoveDocument = async (index: number) => {
  const { isCanceled } = await dialog.reveal();

  !isCanceled && emit("removeDocument", index);
};
</script>

<style lang="scss" scoped>
h2 {
  font-size: var(--font-large);
  margin-block-end: var(--spacing-default);
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

    span {
      font-weight: normal;
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
