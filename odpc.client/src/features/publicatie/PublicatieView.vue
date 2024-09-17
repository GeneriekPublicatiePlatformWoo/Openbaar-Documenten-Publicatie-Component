<template>
  <simple-spinner v-if="isFetching"></simple-spinner>

  <form v-else aria-live="polite" @submit.prevent="submit">
    <alert-inline v-if="error">
      Er is iets misgegaan bij het ophalen van de publicatie, probeer het nogmaals...
    </alert-inline>

    <fieldset v-else-if="publicatie">
      <div class="form-field">
        <label for="titel">Titel</label>

        <input
          id="titel"
          type="text"
          v-model="publicatie.officieleTitel"
          required
          aria-required="true"
        />
      </div>

      <div class="form-field">
        <label for="verkorte_titel">Verkorte titel</label>

        <input id="verkorte_titel" type="text" v-model="publicatie.verkorteTitel" class="small" />
      </div>

      <div class="form-field">
        <label for="omschrijving">Omschrijving</label>

        <textarea id="omschrijving" v-model="publicatie.omschrijving" rows="4"></textarea>
      </div>
    </fieldset>

    <div class="form-submit">
      <router-link :to="{ name: 'publicaties' }" class="button">&lt; Terug</router-link>

      <button v-if="publicatie && !error" type="submit" title="Opslaan">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { onMounted, ref, watch, watchEffect } from "vue";
import { useRouter } from "vue-router";
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import toast from "@/stores/toast";

import type { Publicatie } from "./types";

const router = useRouter();
const props = defineProps<{ id?: string }>();

const publicatie = ref<Publicatie | null>({
  officieleTitel: "",
  verkorteTitel: "",
  omschrijving: "",
  creatiedatum: new Date().toISOString().split("T")[0]
});

const { data, isFetching, error, post, put, execute } = useFetchApi(
  `/api-mock/publicaties${props.id ? "/" + props.id : ""}`,
  { immediate: false }
).json<Publicatie>();

watch(data, () => (publicatie.value = data.value), { immediate: false });

watchEffect(() => {
  !props.id &&
    data.value?.identifier &&
    router.push({ name: "publicatie", params: { id: data.value.identifier } });
});

const submit = async (): Promise<void> => {
  props.id ? put(publicatie) : post(publicatie);

  await execute();

  toast.add(
    error.value
      ? { text: "De gegevens konden niet worden opgeslagen.", type: "error" }
      : { text: "De gegevens zijn succesvol opgeslagen." }
  );
};

onMounted(() => props.id && execute());
</script>

<style lang="scss" scoped>
.form-field {
  display: flex;
  flex-direction: column;
  margin-block-end: var(--spacing-default);

  label {
    font-weight: var(--font-bold);
    margin-block-end: var(--spacing-small);
  }

  input[type="text"].small {
    max-width: var(--section-width);
  }
}

.form-submit {
  display: flex;
  justify-content: flex-end;
  grid-gap: var(--spacing-default);
}
</style>
