<template>
  <h1>{{ !id ? `Nieuwe publicatie` : `Publicatie` }}</h1>

  <publicatie-view :id="id" />
</template>

<script setup lang="ts">
import PublicatieView from "@/features/publicatie/PublicatieView.vue";

defineProps<{ id?: string }>();
</script>

<style lang="scss" scoped></style>
