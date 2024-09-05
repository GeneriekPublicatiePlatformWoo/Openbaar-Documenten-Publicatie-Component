<template>
  <h1>Gebruikersgroep &gt; Groep {{ id }}</h1>

  <p v-if="isFetching" class="notice" aria-live="polite">Bezig met laden...</p>

  <p v-if="error" class="notice">
    Er is iets misgegaan met ophalen van de gegevens, probeer het opnieuw.
  </p>

  <form v-if="!isFetching && !error" aria-live="polite">
    <WaardeLijstView
      v-for="(items, key) in groupedItems"
      :key="key"
      :title="WAARDELIJST[key]"
      :items="items || null"
    />
  </form>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useFetch } from '@vueuse/core'
import { WAARDELIJST, type WaardeLijsten, type WaardeLijstItem } from '@/../mock/api.mock'
import WaardeLijstView from '@/components/WaardeLijstView.vue'

defineProps<{
  id: string
}>()

const waardelijstenUrl = '/api/waardelijsten'

const selectedItems: number[] = [2, 4, 5, 7]

type WaardeLijst = {
  [key in WaardeLijsten]: WaardeLijstItem[]
}

const groupedItems = computed<WaardeLijst>(() =>
  WaardeLijstItems.value?.reduce((result: any, current: WaardeLijstItem) => {
    current.checked = selectedItems.includes(current.id)
    ;(result[current.type] = result[current.type] || []).push(current)
    return result
  }, {})
)

const {
  isFetching,
  error,
  data: WaardeLijstItems
} = await useFetch(waardelijstenUrl).get().json<WaardeLijstItem[]>()
</script>

<style lang="scss" scoped>
form {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(20em, 1fr));
  grid-gap: 1em;
}
</style>
