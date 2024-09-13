<template>
  <h1>Gebruikersgroepen</h1>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-view v-if="error">Er is iets misgegaan, probeer het nogmaals.</alert-view>

  <ul>
    <li v-for="gebruikersgroep in data" :key="gebruikersgroep.id">
      <router-link :to="{ name: 'gebruikersgroep', params: { id: gebruikersgroep.id } }">{{ gebruikersgroep.name
        }}</router-link>
    </li>
  </ul>
</template>

<script setup lang="ts">
import { useFetchApi } from '@/api/use-fetch-api';
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertView from "@/components/AlertView.vue";

const {
  data,
  isFetching,
  error,
} = useFetchApi('/gebruikersgroepen').json<Array<any>>();

</script>

<style lang="scss" scoped>
ul {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width-small), 1fr));
  grid-gap: var(--spacing-default);

  list-style: none;
  padding: 0;
  margin: 0;

  li {
    border: 1px solid var(--color-grey);
    padding: var(--spacing-large);
    text-align: center;
  }
}
</style>
