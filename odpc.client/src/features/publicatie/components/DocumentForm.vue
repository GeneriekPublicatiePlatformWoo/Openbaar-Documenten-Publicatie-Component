<template>
  <prompt-modal :dialog="dialog" confirm-message="Ja, verwijderen" cancel-message="Nee, behouden">
    <p>Weet u zeker dat u dit document wilt verwijderen?</p>
  </prompt-modal>

  <fieldset v-if="model">
    <legend>Documenten</legend>

    <div
      v-if="!model.some((doc) => !doc.uuid)"
      class="dropzone"
      @dragover.prevent="isDragging = true"
      @dragleave.prevent="isDragging = false"
      @drop.prevent="onFilesSelected"
    >
      <label for="bestand">
        <input
          id="bestand"
          type="file"
          multiple
          title="Selecteer bestanden"
          :accept="accept"
          @change="onFilesSelected"
        />

        <span v-if="isDragging">Plaats bestanden hier.</span>
        <span v-else>Selecteer bestanden en sleept ze hierheen of klik hier.</span>
      </label>
    </div>

    <details
      v-for="(doc, index) in model"
      :key="index"
      :class="{ nieuw: !doc.uuid, ingetrokken: doc.status === 'ingetrokken' }"
      :open="!doc.uuid"
    >
      <summary v-if="doc.uuid">{{ doc.bestandsnaam }}</summary>

      <summary v-else @click.prevent tabindex="-1">Nieuw: {{ doc.bestandsnaam }}</summary>

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

      <button
        v-if="doc.uuid"
        type="button"
        class="button icon-after note"
        :class="{ secondary: doc.status === 'gepubliceerd' }"
        @click="$emit('toggleDocument', doc.uuid)"
      >
        Document {{ doc.status === "gepubliceerd" ? "intrekken" : "publiceren" }}
      </button>

      <button
        v-else
        type="button"
        class="button secondary icon-after trash"
        @click="onRemoveDocument(index)"
      >
        Document verwijderen
      </button>
    </details>
  </fieldset>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useConfirmDialog } from "@vueuse/core";
import toast from "@/stores/toast";
import PromptModal from "@/components/PromptModal.vue";
import type { PublicatieDocument } from "../types";
import { mimeTypesMap } from "../service";

const props = defineProps<{ documenten: PublicatieDocument[] }>();

const emit = defineEmits<{
  (e: "update:documenten", payload: PublicatieDocument[]): void;
  (e: "update:files", payload: File[]): void;
  (e: "removeDocument", payload: number): void;
  (e: "toggleDocument", payload: string): void;
}>();

const model = computed({
  get: () => props.documenten,
  set: (value) => emit("update:documenten", value)
});

const dialog = useConfirmDialog();

const accept = computed(() => Array.from(mimeTypesMap.value?.keys() || []).join(","));

const isDragging = ref(false);

const onFilesSelected = (event: Event | DragEvent) => {
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
.dropzone {
  padding: var(--spacing-large);
  margin-block-end: var(--spacing-default);
  background: #f7fafc;
  border: 2px solid #e2e8f0;
}

button {
  display: flex;
  column-gap: var(--spacing-small);
}

details {
  &.nieuw {
    summary {
      list-style: none;

      &::-webkit-details-marker {
        display: none;
      }
    }
  }

  &.ingetrokken {
    background-color: #eee;

    span {
      font-weight: normal;
    }
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
