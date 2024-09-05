<template>
  <h1>Gebruikersgroep &gt; Groep {{ id }}</h1>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-view v-else-if="error"></alert-view>

  <form v-else aria-live="polite" @submit.prevent="submit">
    <section>
      <WaardelijstView
        v-for="(value, key) in WAARDELIJSTEN"
        :key="key"
        :title="value"
        :items="groupedItems?.[key] || null"
        v-model="selectedItems"
      />
    </section>

    <div class="form-submit">
      <button type="submit" title="Opslaan">Opslaan</button>
      <router-link :to="{ name: 'gebruikersgroepen' }" class="button">Annuleren</router-link>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useFetch } from '@vueuse/core'
import SimpleSpinner from '@/components/SimpleSpinner.vue'
import AlertView from '@/components/AlertView.vue'
import {
  WAARDELIJSTEN,
  type WaardelijstItem,
  type GroupedWaardeLijstItems
} from '@/../mock/api.mock'
import WaardelijstView from '@/components/WaardelijstView.vue'

defineProps<{
  id: string
}>()

const waardelijstenUrl = '/api/waardelijsten'
const selectedItems = ref<number[]>([2, 4, 5, 7])

const groupedItems = computed<GroupedWaardeLijstItems | null>(
  () =>
    waardeLijstItems.value?.reduce((result: GroupedWaardeLijstItems, current: WaardelijstItem) => {
      ;(result[current.type] = result[current.type] || []).push(current)
      result[current.type].sort((a, b) => a.name.localeCompare(b.name))
      return result
    }, {} as GroupedWaardeLijstItems) || null
)

const submit = (): void => {
  console.log('submit', selectedItems.value)
}

const {
  isFetching,
  error,
  data: waardeLijstItems
} = useFetch(waardelijstenUrl).get().json<WaardelijstItem[]>()
</script>

<style lang="scss" scoped>
section {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width-small), 1fr));
  grid-gap: var(--spacing-default);

  // ...
  padding: 0;
  margin: 0;
  border: none;
}

.form-submit {
  display: flex;
  justify-content: flex-end;
  grid-gap: var(--spacing-default);
}
</style>
