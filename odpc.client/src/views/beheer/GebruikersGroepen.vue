<template>
  <ul>
    <li>
      <router-link :to="{ name: 'gebruikersgroep', params: { id: 1 } }"
        >Gebruikersgroep 1</router-link
      >
    </li>

    <li>
      <router-link :to="{ name: 'gebruikersgroep', params: { id: 2 } }"
        >Gebruikersgroep 2</router-link
      >
    </li>

    <li>
      <router-link :to="{ name: 'gebruikersgroep', params: { id: 3 } }"
        >Gebruikersgroep 3</router-link
      >
    </li>
  </ul>
</template>

<script setup lang="ts"></script>

<style lang="scss" scoped>
ul {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(20em, 1fr));
  grid-gap: 1em;

  list-style: none;
  padding: 0;
  margin: 0;

  li {
    border: 1px solid var(--border);
    padding: 2rem;
    text-align: center;
  }
}
</style>
