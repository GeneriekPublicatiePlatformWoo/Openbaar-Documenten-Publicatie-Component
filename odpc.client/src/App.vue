<script setup lang="ts">
import { RouterView } from "vue-router";
import ToastSection from "./components/ToastSection.vue";
import HeaderView from "./components/HeaderView.vue";
import FooterView from "./components/FooterView.vue";
</script>

<template>
  <toast-section />

  <Suspense>
    <header-view />
  </Suspense>

  <main>
    <router-view />
  </main>

  <footer-view />
</template>
