<template>
  <p>
    <router-link :to="{ name: 'publicatie' }" class="button">Nieuwe publicatie</router-link>
  </p>

  <form>
    <fieldset>
      <div class="form-group form-group-button">
        <label for="zoeken">Zoeken</label>

        <input type="text" id="zoeken" v-model="searchString" @keydown.enter.prevent="onSearch" />

        <button
          type="button"
          class="icon-after loupe"
          aria-label="Zoeken"
          @click="onSearch"
        ></button>
      </div>
    </fieldset>

    <date-range-picker
      v-model:fromDate="queryParams.registratiedatum__gte"
      v-model:untilDate="queryParams.registratiedatum__lte"
    />

    <fieldset>
      <div class="form-group">
        <label for="sorteer">Sorteer op</label>

        <select name="sorteer" id="sorteer" v-model="queryParams.sorteer">
          <option
            v-for="(value, key) in {
              officiele_titel: 'Title (a-z)',
              '-officiele_titel': 'Title (z-a)',
              verkorte_titel: 'Verkorte title (a-z)',
              '-verkorte_titel': 'Verkorte title (z-a)',
              registratiedatum: 'Registratiedatum (oud-nieuw)',
              '-registratiedatum': 'Registratiedatum (nieuw-oud)'
            }"
            :key="key"
            :value="key"
          >
            {{ value }}
          </option>
        </select>
      </div>
    </fieldset>
  </form>

  <div class="page-bar">
    <p>
      <strong>{{ pagedResult?.count || 0 }}</strong> resultaten
    </p>

    <p>
      <button type="button" :disabled="!pagedResult?.previous" @click="onPrev">&laquo;</button>

      <span>pagina {{ queryParams.page }} van {{ pageCount }}</span>

      <button type="button" :disabled="!pagedResult?.next" @click="onNext">&raquo;</button>
    </p>
  </div>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error"
    >Er is iets misgegaan, probeer het nogmaals. {{ error }}</alert-inline
  >

  <ul v-else class="reset" aria-live="polite">
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
</template>

<script setup lang="ts">
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import DateRangePicker from "@/components/DateRangePicker.vue";
import { usePagedSearch } from "@/composables/use-paged-search";
import { publicatieSearchParams, type Publicatie, type PublicatieSearchParam } from "./types";

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

.page-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;

  p {
    margin-block: var(--spacing-default);
  }

  button {
    padding-block: var(--spacing-extrasmall);
  }

  span {
    margin-inline: var(--spacing-default);
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
