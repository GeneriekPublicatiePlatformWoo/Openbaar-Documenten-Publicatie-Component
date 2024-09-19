<template>
  <h1>{{ !id ? `Nieuwe publicatie` : `Publicatie` }}</h1>

  <publicatie-details :id="id" />
</template>

<script setup lang="ts">
import PublicatieDetails from "@/features/publicatie/PublicatieDetails.vue";

defineProps<{ id?: string }>();
</script>

<style lang="scss" scoped></style>
