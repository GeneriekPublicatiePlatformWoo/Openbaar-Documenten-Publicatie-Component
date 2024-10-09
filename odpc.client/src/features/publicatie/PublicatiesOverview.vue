<template>
  <p>
    <router-link :to="{ name: 'publicatie' }" class="button">Nieuwe publicatie</router-link>
  </p>

  <div class="search">
    <div class="form-group form-group-button">
      <label for="zoeken">Zoeken</label>

      <input type="text" id="zoeken" v-model="queryString" @keydown.enter.prevent="onSearch" />

      <button type="button" class="icon-after loupe" aria-label="Zoeken" @click="onSearch"></button>
    </div>

    <div class="form-group">
      <label for="van">Van</label>

      <input type="date" id="van" v-model="queryParams.van" />
    </div>

    <div class="form-group">
      <label for="tot">Tot</label>

      <input type="date" id="tot" v-model="queryParams.tot" />
    </div>

    <div class="form-group">
      <label for="sorteer">Sorteer op</label>

      <select name="sorteer" id="sorteer" v-model="queryParams.sorteer">
        <option value=""></option>
      </select>
    </div>
  </div>

  <div class="search">
    <p><strong>{{ publicaties?.length }}</strong> resultaten</p>

    <p>Pagina {{ queryParams.page }} van 456</p>
  </div>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error"
    >Er is iets misgegaan, probeer het nogmaals. {{ error }}</alert-inline
  >

  <ul v-else class="reset">
    <li v-for="{ uuid, officieleTitel, creatiedatum } in publicaties" :key="uuid">
      <router-link
        :to="{ name: 'publicatie', params: { uuid } }"
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
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import { PublicatieSearchParams, type Publicatie, type PublicatieSearchParam } from "./types";
import { useSearch } from "./use-search";

const {
  items: publicaties,
  queryString,
  queryParams,
  onSearch,
  isFetching,
  error
} = useSearch<Publicatie[], PublicatieSearchParam>("publicaties", {
  ...PublicatieSearchParams,
  page: "1",
  sorteer: "registratiedatum"
});
</script>

<style lang="scss" scoped>
.search {
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
}

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
