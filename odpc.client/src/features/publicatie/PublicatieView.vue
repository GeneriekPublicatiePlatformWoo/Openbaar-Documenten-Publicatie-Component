<template>
  <simple-spinner v-if="isFetching"></simple-spinner>

  <form v-else aria-live="polite" @submit.prevent="submit">
    <alert-inline v-if="error">Er is iets misgegaan, probeer het nogmaals...</alert-inline>

    <fieldset v-else-if="publicatie">
      <div class="form-group">
        <label for="titel">Titel</label>

        <input
          id="titel"
          type="text"
          v-model="publicatie.officieleTitel"
          required
          aria-required="true"
        />
      </div>

      <div class="form-group">
        <label for="verkorte_titel">Verkorte titel</label>

        <input id="verkorte_titel" type="text" v-model="publicatie.verkorteTitel" class="small" />
      </div>

      <div class="form-group">
        <label for="omschrijving">Omschrijving</label>

        <textarea id="omschrijving" v-model="publicatie.omschrijving" rows="4"></textarea>
      </div>
    </fieldset>

    <div class="form-submit" :class="{ error }">
      <router-link :to="{ name: 'publicaties' }" class="button">{{
        error ? "&lt; Terug" : "Annuleren"
      }}</router-link>

      <button v-if="publicatie && !error" type="submit" title="Opslaan">Opslaan</button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { onMounted, ref, watch } from "vue";
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
  () => `/api-mock/publicaties${props.id ? "/" + props.id : ""}`,
  { immediate: false }
).json<Publicatie>();

watch(data, () => (publicatie.value = data.value), { immediate: false });

const submit = async (): Promise<void> => {
  props.id ? put(publicatie) : post(publicatie);

  await execute();

  toast.add(
    error.value
      ? { text: "De gegevens konden niet worden opgeslagen.", type: "error" }
      : { text: "De gegevens zijn succesvol opgeslagen." }
  );

  !error.value && router.push({ name: "publicaties" });
};

onMounted(() => props.id && execute());
</script>

<style lang="scss" scoped></style>
