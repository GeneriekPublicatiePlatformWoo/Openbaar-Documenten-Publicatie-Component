<template>
  <p class="notice" aria-live="polite" role="alert">
    Er is iets misgegaan bij het ophalen van de gegevens, probeer het opnieuw.
  </p>
</template>

<script setup lang="ts"></script>

<style lang="scss" scoped>
.notice {
  border-color: var(--color-error);
}
</style>
