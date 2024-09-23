<template>
  <h1>{{ !uuid ? `Nieuwe publicatie` : `Publicatie` }}</h1>

  <publicatie-details :uuid="uuid" />
</template>

<script setup lang="ts">
import PublicatieDetails from "@/features/publicatie/PublicatieDetails.vue";

defineProps<{ uuid?: string }>();
</script>

<style lang="scss" scoped></style>
