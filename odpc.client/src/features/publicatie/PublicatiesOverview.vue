<template>
  <p>
    <router-link :to="{ name: 'publicatie' }" class="button">Nieuwe publicatie</router-link>
  </p>

  <form>
    <fieldset :disabled="isFetching">
      <legend>Zoek op</legend>

      <div class="form-group">
        <label for="zoeken">Titel</label>

        <input type="text" id="zoeken" v-model="searchString" @keydown.enter.prevent="onSearch" />
      </div>

      <date-range-picker v-model:from-date="fromDate" v-model:until-date="untilDate" />

      <div class="form-group-button">
        <button type="button" class="icon-after loupe" aria-label="Zoek" @click="onSearch">
          Zoek
        </button>
      </div>
    </fieldset>
  </form>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-inline v-else-if="error"
    >Er is iets misgegaan, probeer het nogmaals. {{ error }}</alert-inline
  >

  <template v-else>
    <section>
      <div class="form-group">
        <label for="sorteer">Sorteer op</label>

        <select name="sorteer" id="sorteer" v-model="queryParams.sorteer">
          <option v-for="(value, key) in sortingOptions" :key="key" :value="key">
            {{ value }}
          </option>
        </select>
      </div>

      <div v-if="pageCount" class="page-nav">
        <p>
          <strong>{{ pagedResult?.count || 0 }}</strong>
          {{ pagedResult?.count === 1 ? "resultaat" : "resultaten" }}
        </p>

        <p>
          <button type="button" :disabled="!pagedResult?.previous" @click="onPrev">&laquo;</button>

          <span>pagina {{ queryParams.page }} van {{ pageCount }}</span>

          <button type="button" :disabled="!pagedResult?.next" @click="onNext">&raquo;</button>
        </p>
      </div>
    </section>

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
import { ref, watch } from "vue";
import SimpleSpinner from "@/components/SimpleSpinner.vue";
import AlertInline from "@/components/AlertInline.vue";
import DateRangePicker from "@/components/DateRangePicker.vue";
import { usePagedSearch } from "@/composables/use-paged-search";
import { type Publicatie } from "./types";

const searchString = ref("");
const fromDate = ref<string>("");
const untilDate = ref<string>("");

const sortingOptions = {
  officiele_titel: "Title (a-z)",
  "-officiele_titel": "Title (z-a)",
  verkorte_titel: "Verkorte title (a-z)",
  "-verkorte_titel": "Verkorte title (z-a)",
  registratiedatum: "Registratiedatum (oud-nieuw)",
  "-registratiedatum": "Registratiedatum (nieuw-oud)"
};

const searchParamsConfig = {
  page: "1",
  sorteer: "-registratiedatum",
  search: "",
  registratiedatum__gte: "",
  registratiedatum__lte: ""
};

const { pagedResult, queryParams, pageCount, onNext, onPrev, isFetching, error } = usePagedSearch<
  Publicatie,
  keyof typeof searchParamsConfig
>("publicaties", searchParamsConfig);

// Set refs from urlQueryParams/config once on mounted
watch(
  () => ({
    search: queryParams.value.search,
    registratiedatum__gte: queryParams.value.registratiedatum__gte,
    registratiedatum__lte: queryParams.value.registratiedatum__lte
  }),
  ({ search, registratiedatum__gte, registratiedatum__lte }) => {
    searchString.value = search;
    fromDate.value = registratiedatum__gte;
    untilDate.value = registratiedatum__lte;
  },
  { once: true }
);

// Set queryParams linked to refs on search
const onSearch = () =>
  (queryParams.value = {
    ...queryParams.value,
    search: searchString.value,
    registratiedatum__gte: fromDate.value,
    registratiedatum__lte: untilDate.value
  });
</script>

<style lang="scss" scoped>
fieldset {
  display: flex;
  flex-wrap: wrap;
  align-items: flex-end;
  column-gap: var(--spacing-default);

  .form-group {
    flex-grow: 1;
    margin-block-end: 0;
  }
}

section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width), 1fr));
  column-gap: var(--spacing-extralarge);
}

.page-nav {
  display: flex;
  justify-content: flex-end;
  align-items: center;
  column-gap: var(--spacing-large);

  p {
    margin-block: var(--spacing-large);
  }

  button {
    padding-block: var(--spacing-extrasmall);
    margin-block: 0;
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
