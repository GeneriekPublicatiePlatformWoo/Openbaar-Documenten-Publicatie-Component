<template>
  <h1>Gebruikersgroep</h1>

  <gebruikers-groep :id="id" />
</template>

<script setup lang="ts">
import GebruikersGroep from "@/features/gebruikersgroep/GebruikersGroep.vue";

defineProps<{ id: string }>();
</script>

<style lang="scss" scoped></style>
