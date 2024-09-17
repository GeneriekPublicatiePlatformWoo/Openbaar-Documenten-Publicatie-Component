<script setup lang="ts">
import { RouterView } from "vue-router";
import ToastSection from "./components/ToastSection.vue";
import TheHeader from "./components/TheHeader.vue";
import TheFooter from "./components/TheFooter.vue";
</script>

<template>
  <toast-section />

  <Suspense>
    <the-header />
  </Suspense>

  <main>
    <router-view />
  </main>

  <the-footer />
</template>
