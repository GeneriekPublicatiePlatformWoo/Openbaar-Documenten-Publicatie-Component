<template>
  <div class="page-nav">
    <p>
      <strong>{{ pagedResult?.count || 0 }}</strong>
      {{ pagedResult?.count === 1 ? "resultaat" : "resultaten" }}
    </p>

    <p>
      <button type="button" :disabled="!pagedResult?.previous" @click="$emit('onPrev')">
        &laquo;
      </button>

      <span>pagina {{ page }} van {{ pageCount }}</span>

      <button type="button" :disabled="!pagedResult?.next" @click="$emit('onNext')">&raquo;</button>
    </p>
  </div>
</template>

<script setup lang="ts" generic="T">
import type { PagedResult } from "../composables/use-paged-search";

defineProps<{ pagedResult: PagedResult<T> | null; page: string; pageCount: number }>();

defineEmits<{ (e: "onPrev"): void; (e: "onNext"): void }>();
</script>

<style lang="scss" scoped>
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
</style>
