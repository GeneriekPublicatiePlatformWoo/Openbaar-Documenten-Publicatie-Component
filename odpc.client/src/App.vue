<script setup lang="ts">
import { RouterView } from "vue-router";
import ToastSection from "./components/ToastSection.vue";
import TheHeader from "./layout/TheHeader.vue";
import TheFooter from "./layout/TheFooter.vue";
</script>

<template>
  <toast-section />
  <the-header />
  <main>
    <router-view />
  </main>
  <the-footer />
</template>
