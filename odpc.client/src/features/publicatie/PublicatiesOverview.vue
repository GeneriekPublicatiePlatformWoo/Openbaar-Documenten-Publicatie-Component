<template>
  <p>
    <router-link :to="{ name: 'publicatie' }" class="button">Nieuwe publicatie</router-link>
  </p>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error">Er is iets misgegaan, probeer het nogmaals.</alert-inline>

  <ul v-else class="reset">
    <li v-for="{ uuid, officieleTitel, creatiedatum } in data" :key="uuid">
      <router-link
        :to="{ name: 'publicatie', params: { id: uuid } }"
        :title="officieleTitel"
        class="card-link icon-after pen"
      >
        <h2>{{ officieleTitel }}</h2>

        <dl>
          <dt>Publicatiedatum</dt>
          <dd>{{ creatiedatum }}</dd>
        </dl>
      </router-link>
    </li>
  </ul>
</template>

<script setup lang="ts">
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import type { Publicatie } from "./types";

const { data, isFetching, error } = useFetchApi("/api-mock/v1/publicaties").json<Publicatie[]>();
</script>

<style lang="scss" scoped>
ul {
  display: flex;
  flex-direction: column;
  gap: var(--spacing-default);
}

dl {
  display: flex;
  margin-block: var(--spacing-default) 0;

  dd {
    color: var(--text-light);
    margin-inline-start: var(--spacing-extrasmall);
  }
}
</style>
