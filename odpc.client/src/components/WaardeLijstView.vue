<template>
  <fieldset>
    <legend>{{ title }}</legend>

    <div class="form-field" v-for="({ id, name }, key) in items" :key="key">
      <label><input type="checkbox" :value="id" v-model="model" /> {{ name }}</label>
    </div>
  </fieldset>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { type WaardelijstItem } from '@/../mock/api.mock'

const props = defineProps<{
  title: string
  items: WaardelijstItem[] | null
  modelValue: number[]
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', payload: number[]): void
}>()

const model = computed({
  get: () => props.modelValue,
  set: (value) => emit('update:modelValue', value)
})
</script>

<style lang="scss" scoped></style>
