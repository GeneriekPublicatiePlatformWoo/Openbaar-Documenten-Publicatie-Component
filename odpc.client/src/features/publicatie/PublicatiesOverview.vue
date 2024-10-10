<template>
  <p>
    <router-link :to="{ name: 'publicatie' }" class="button">Nieuwe publicatie</router-link>
  </p>

  <div class="search">
    <div class="form-group form-group-button">
      <label for="zoeken">Zoeken</label>

      <input type="text" id="zoeken" v-model="searchString" @keydown.enter.prevent="onSearch" />

      <button type="button" class="icon-after loupe" aria-label="Zoeken" @click="onSearch"></button>
    </div>

    <date-range-picker
      v-model:fromDate="queryParams.registratiedatum__gte"
      v-model:untilDate="queryParams.registratiedatum__lte"
    />

    <div class="form-group">
      <label for="sorteer">Sorteer op</label>

      <select name="sorteer" id="sorteer" v-model="queryParams.sorteer">
        <option
          v-for="(value, index) in ['officiele_titel', 'verkorte_titel', 'registratiedatum']"
          :key="index"
          :value="value"
        >
          {{ value }}
        </option>
      </select>
    </div>
  </div>

  <div class="search">
    <p>
      <strong>{{ pagedResult?.count || 0 }}</strong> resultaten
    </p>

    <p>
      <button type="button" :disabled="!pagedResult?.previous" @click="onPrev">&laquo;</button>
      pagina {{ queryParams.page }} van {{ pageCount }}
      <button type="button" :disabled="!pagedResult?.next" @click="onNext">&raquo;</button>
    </p>
  </div>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error"
    >Er is iets misgegaan, probeer het nogmaals. {{ error }}</alert-inline
  >

  <ul v-else class="reset">
    <li v-for="{ uuid, officieleTitel, registratiedatum } in pagedResult?.results" :key="uuid">
      <router-link
        :to="{ name: 'publicatie', params: { uuid } }"
        :title="officieleTitel"
        class="card-link icon-after pen"
      >
        <h2>{{ officieleTitel }}</h2>

        <dl>
          <dt>Publicatiedatum</dt>
          <dd>{{ registratiedatum }}</dd>
        </dl>
      </router-link>
    </li>
  </ul>
</template>

<script setup lang="ts">
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import DateRangePicker from "@/components/DateRangePicker.vue";
import { usePagedSearch } from "@/composables/use-paged-search";
import { publicatieSearchParams, type Publicatie, type PublicatieSearchParam } from "./types";

const { pagedResult, pageCount, searchString, queryParams, onSearch, onNext, onPrev, isFetching, error } =
  usePagedSearch<Publicatie, PublicatieSearchParam>("publicaties", publicatieSearchParams);
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
