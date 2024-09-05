<template>
  <fieldset>
    <legend>{{ title }}</legend>

    <div class="formfield" v-for="({ id, type, name, checked }, key) in items" :key="key">
      <label
        ><input :name="type" type="checkbox" :value="id" :checked="checked" /> {{ name }}</label
      >
    </div>
  </fieldset>
</template>

<script setup lang="ts">
import { type WaardeLijstItem } from '@/../mock/api.mock'

defineProps<{
  title: string
  items: WaardeLijstItem[] | null
}>()
</script>

<style lang="scss" scoped></style>
