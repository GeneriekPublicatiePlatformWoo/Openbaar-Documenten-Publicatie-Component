<template>
  <p>
    <router-link :to="{ name: 'publicatie' }" class="button">Nieuwe publicatie</router-link>
  </p>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error">Er is iets misgegaan, probeer het nogmaals.</alert-inline>

  <ul v-else>
    <li v-for="{ identifier, officieleTitel, creatiedatum } in data" :key="identifier">
      <router-link :to="{ name: 'publicatie', params: { id: identifier } }">
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

const { data, isFetching, error } = useFetchApi("/api-mock/publicaties").json<Publicatie[]>();
</script>

<style lang="scss" scoped>
ul {
  list-style: none;
  padding: 0;
  margin: 0;

  li {
    margin-block: var(--spacing-default);
  }

  h2 {
    margin-block-start: 0;
  }

  a {
    display: flex;
    flex-direction: column;

    color: var(--text);
    text-decoration: none;
    padding: var(--spacing-large);
    border: 1px solid var(--color-grey);

    h2 {
      text-decoration: underline;
    }

    &:hover h2 {
      text-decoration: none;
    }
  }
}

dl {
  display: flex;
  margin-block: 0;

  dd {
    color: var(--text-light);
    margin-inline-start: var(--spacing-extrasmall);
  }
}
</style>
