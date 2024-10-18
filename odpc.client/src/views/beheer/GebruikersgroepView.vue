<template>
  <h1>Gebruikersgroep</h1>

  <gebruikersgroep-details :uuid="uuid" />
</template>

<script setup lang="ts">
import GebruikersgroepDetails from "@/features/gebruikersgroep/GebruikersgroepDetails.vue";

defineProps<{ uuid?: string }>();
</script>

<style lang="scss" scoped></style>
