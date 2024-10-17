<template>
  <fieldset v-if="model" aria-live="polite" :disabled="disabled">
    <legend>Documenten</legend>

    <template v-if="!model.some((doc) => !doc.uuid) && !disabled">
      <file-upload @filesSelected="filesSelected" />

      <h2 v-if="model.length">Toegevoegde documenten</h2>
    </template>

    <details
      v-for="(doc, index) in model"
      :key="index"
      :class="{
        nieuw: !doc.uuid
      }"
      :open="!doc.uuid"
    >
      <template v-if="doc.uuid">
        <summary>
          <template v-if="doc.status === PublicatieStatus.ingetrokken || disabled"
            ><s :aria-describedby="`status-${index}`">{{ doc.bestandsnaam }}</s>
            <span :id="`status-${index}`" role="status">(ingetrokken)</span></template
          >
          <template v-else>{{ doc.bestandsnaam }}</template>
        </summary>

        <div v-if="!disabled" class="form-group form-group-radio">
          <label>
            <input type="radio" v-model="doc.status" :value="PublicatieStatus.gepubliceerd" />
            Gepubliceerd
          </label>

          <label
            ><input type="radio" v-model="doc.status" :value="PublicatieStatus.ingetrokken" />
            Ingetrokken</label
          >
        </div>
      </template>

      <summary v-else @click.prevent tabindex="-1">
        Nieuw: <span>{{ doc.bestandsnaam }}</span>
      </summary>

      <div class="form-group">
        <label :for="`document_titel_${index}`">Titel *</label>

        <input
          :id="`document_titel_${index}`"
          type="text"
          v-model="doc.officieleTitel"
          required
          aria-required="true"
          :aria-describedby="`titelError-${index}`"
          :aria-invalid="!doc.officieleTitel"
        />

        <span :id="`titelError-${index}`" class="error">Titel is een verplicht veld</span>
      </div>

      <div class="form-groups">
        <div class="form-group">
          <label :for="`documenthandeling_${index}`">Documenthandeling</label>

          <select
            :name="`documenthandeling_${index}`"
            :id="`documenthandeling_${index}`"
            v-model="doc.documenthandeling.soortHandeling"
          >
            <option v-for="(value, key) in Documenthandelingen" :key="key" :value="key">
              {{ value }}
            </option>
          </select>
        </div>

        <div class="form-group">
          <label :for="`tijdstip_${index}`">Datum</label>

          <input type="date" :id="`tijdstip_${index}`" v-model="doc.documenthandeling.tijdstip" :max="today" />
        </div>
      </div>

      <details>
        <summary>Meer details</summary>

        <div class="form-group">
          <label :for="`verkorte_titel_${index}`">Verkorte titel</label>

          <input :id="`verkorte_titel_${index}`" type="text" v-model="doc.verkorteTitel" />
        </div>

        <div class="form-group">
          <label :for="`omschrijving_${index}`">Omschrijving</label>

          <textarea :id="`omschrijving_${index}`" v-model="doc.omschrijving" rows="4"></textarea>
        </div>
      </details>

      <button
        v-if="!doc.uuid"
        type="button"
        class="button secondary icon-after trash"
        @click="onRemoveDocument(index)"
      >
        Verwijderen
      </button>
    </details>

    <alert-inline v-if="disabled && !model.length"
      >Er zijn geen gekoppelde documenten.</alert-inline
    >

    <prompt-modal :dialog="dialog" confirm-message="Ja, verwijderen" cancel-message="Nee, behouden">
      <p>Weet u zeker dat u dit document wilt verwijderen?</p>
    </prompt-modal>
  </fieldset>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useConfirmDialog } from "@vueuse/core";
import toast from "@/stores/toast";
import AlertInline from "@/components/AlertInline.vue";
import PromptModal from "@/components/PromptModal.vue";
import { PublicatieStatus, Documenthandelingen, type PublicatieDocument } from "../types";
import { mimeTypesMap } from "../service";
import FileUpload from "./FileUpload.vue";

const props = defineProps<{ documenten: PublicatieDocument[]; disabled: boolean }>();

const emit = defineEmits<{
  (e: "update:documenten", payload: PublicatieDocument[]): void;
  (e: "update:files", payload: File[]): void;
  (e: "removeDocument", payload: number): void;
}>();

const model = computed<PublicatieDocument[]>({
  get: () => props.documenten,
  set: (value) => emit("update:documenten", value)
});

const today = ref(new Date().toISOString().split("T")[0]);

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

details {
  span {
    font-weight: normal;
    margin-inline-start: var(--spacing-extrasmall);
  }

  fieldset > &.nieuw { // ...
    & > summary {
      list-style: none;
      pointer-events: none;

      &::-webkit-details-marker {
        display: none;
      }
    }
  }

  &.ingetrokken {
    background-color: var(--disabled);
  }

  & > details {
    background-color: #fff;
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

.form-groups {
  display: flex;
  gap: var(--spacing-default);

  .form-group {
    flex-grow: 1;
  }
}
</style>
