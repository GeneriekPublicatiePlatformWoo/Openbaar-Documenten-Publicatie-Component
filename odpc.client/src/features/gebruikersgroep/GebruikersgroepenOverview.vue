<template>
  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error">Er is iets misgegaan, probeer het nogmaals.</alert-inline>

  <ul v-else class="reset">
    <li v-for="{ id, name } in data" :key="id">
      <router-link
        :to="{ name: 'gebruikersgroep', params: { id } }"
        :title="name"
        class="card-link icon-after pen"
      >
        <h2>{{ name }}</h2></router-link
      >
    </li>
  </ul>
</template>

<script setup lang="ts">
import { useFetchApi } from "@/api/use-fetch-api";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import type { Gebruikersgroep } from "./types";

const { data, isFetching, error } = useFetchApi("/api/gebruikersgroepen").json<Gebruikersgroep[]>();
</script>

<style lang="scss" scoped>
ul {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width-small), 1fr));
  grid-gap: var(--spacing-default);
}

.card-link {
  align-items: center;
}
</style>
