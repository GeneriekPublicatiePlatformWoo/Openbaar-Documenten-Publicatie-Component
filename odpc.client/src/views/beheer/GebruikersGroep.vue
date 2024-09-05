<template>
  <h1>Gebruikersgroep &gt; Groep {{ id }}</h1>

  <simple-spinner v-if="isFetching"></simple-spinner>

  <alert-view v-if="error"></alert-view>

  <p v-if="error" class="notice">
    Er is iets misgegaan met ophalen van de gegevens, probeer het opnieuw.
  </p>

  <form v-if="!isFetching && !error" aria-live="polite">
    <WaardelijstView
      v-for="(value, key) in WAARDELIJST"
      :key="key"
      :title="value"
      :items="groupedItems?.[key] || null"
    />
  </form>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useFetch } from '@vueuse/core'
import SimpleSpinner from '@/components/SimpleSpinner.vue'
import AlertView from '@/components/AlertView.vue'
import { WAARDELIJST, type Waardelijsten, type WaardelijstItem } from '@/../mock/api.mock'
import WaardelijstView from '@/components/WaardelijstView.vue'

defineProps<{
  id: string
}>()

const waardelijstenUrl = '/api/waardelijsten'

const selectedItems: number[] = [2, 4, 5, 7]

type WaardeLijst = {
  [key in Waardelijsten]: WaardelijstItem[]
}

const groupedItems = computed<WaardeLijst | null>(
  () =>
    WaardeLijstItems.value?.reduce((result: WaardeLijst, current: WaardelijstItem) => {
      current.checked = selectedItems.includes(current.id)
      ;(result[current.type] = result[current.type] || []).push(current)
      result[current.type].sort((a, b) => a.name.localeCompare(b.name))
      return result
    }, {} as WaardeLijst) || null
)

const {
  isFetching,
  error,
  data: WaardeLijstItems
} = await useFetch(waardelijstenUrl).get().json<WaardelijstItem[]>()
</script>

<style lang="scss" scoped>
form {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(var(--section-width-small), 1fr));
  grid-gap: var(--spacing-default);
}
</style>
