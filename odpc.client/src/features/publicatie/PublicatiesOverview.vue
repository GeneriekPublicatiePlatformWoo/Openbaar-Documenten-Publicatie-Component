<template>
  <p>
    <router-link :to="{ name: 'publicatie' }" class="button">Nieuwe publicatie</router-link>
  </p>

  <form>
    <fieldset :disabled="isFetching">
      <div class="form-group form-group-button">
        <label for="zoeken">Zoek op titel</label>

        <input type="text" id="zoeken" v-model="searchString" @keydown.enter.prevent="onSearch" />

        <button type="button" class="icon-after loupe" aria-label="Zoek" @click="onSearch"></button>
      </div>
    </fieldset>

    <date-range-picker
      :disabled="isFetching"
      v-model:from-date="queryParams.registratiedatum__gte"
      v-model:until-date="queryParams.registratiedatum__lte"
    />

    <fieldset :disabled="isFetching">
      <div class="form-group">
        <label for="sorteer">Sorteer op</label>

        <select name="sorteer" id="sorteer" v-model="queryParams.sorteer">
          <option v-for="(value, key) in publicatieSortingOptions" :key="key" :value="key">
            {{ value }}
          </option>
        </select>
      </div>
    </fieldset>
  </form>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error"
    >Er is iets misgegaan, probeer het nogmaals. {{ error }}</alert-inline
  >

  <template v-else>
    <page-nav
      v-if="pageCount"
      :paged-result="pagedResult"
      :page="queryParams.page"
      :page-count="pageCount"
      @on-next="onNext"
      @on-prev="onPrev"
    />

    <ul v-if="pagedResult?.results.length" class="reset" aria-live="polite">
      <li
        v-for="{ uuid, officieleTitel, verkorteTitel, registratiedatum } in pagedResult?.results"
        :key="uuid"
      >
        <router-link
          :to="{ name: 'publicatie', params: { uuid } }"
          :title="officieleTitel"
          class="card-link icon-after pen"
        >
          <h2>{{ officieleTitel }}</h2>

          <h3>{{ verkorteTitel }}</h3>

          <dl>
            <dt>Publicatiedatum:</dt>
            <dd>
              {{
                Intl.DateTimeFormat("default", { dateStyle: "long" }).format(
                  Date.parse(registratiedatum)
                )
              }}
            </dd>
          </dl>
        </router-link>
      </li>
    </ul>

    <p v-else>Geen publicaties gevonden.</p>
  </template>
</template>

<script setup lang="ts">
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import DateRangePicker from "@/components/DateRangePicker.vue";
import PageNav from "./components/PageNav.vue";
import { usePagedSearch } from "@/features/publicatie/composables/use-paged-search";
import {
  publicatieSearchParams,
  publicatieSortingOptions,
  type Publicatie,
  type PublicatieSearchParam
} from "./types";

const {
  pagedResult,
  pageCount,
  searchString,
  queryParams,
  onSearch,
  onNext,
  onPrev,
  isFetching,
  error
} = usePagedSearch<Publicatie, PublicatieSearchParam>("publicaties", publicatieSearchParams);
</script>

<style lang="scss" scoped>
form {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width-small), 1fr));
  column-gap: var(--spacing-large);

  fieldset {
    margin: 0;
    padding: 0;
    border: none;
  }
}

ul {
  display: flex;
  flex-direction: column;
  gap: var(--spacing-default);
}

dl {
  display: flex;
  margin-block: var(--spacing-small) 0;

  dd {
    color: var(--text-light);
    margin-inline-start: var(--spacing-extrasmall);
  }
}
</style>
