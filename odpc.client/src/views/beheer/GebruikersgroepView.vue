<template>
  <h1>Gebruikersgroep</h1>

  <gebruikersgroep-details :id="id" />
</template>

<script setup lang="ts">
import GebruikersgroepDetails from "@/features/gebruikersgroep/GebruikersgroepDetails.vue";

defineProps<{ id: string }>();
</script>

<style lang="scss" scoped></style>
