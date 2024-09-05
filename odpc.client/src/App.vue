<script setup lang="ts">
import { RouterView } from 'vue-router'
import TheHeader from './layout/TheHeader.vue'
import TheFooter from './layout/TheFooter.vue'
</script>

<template>
  <the-header />
  <main>
    <router-view />
  </main>
  <the-footer />
</template>
