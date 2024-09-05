<template>
  <header>
    <h1>Openbare Documenten Publicatie Component</h1>

    <nav>
      <ul>
        <li>
          <router-link :to="{ name: 'home' }">Home</router-link>
        </li>

        <li>
          <router-link :to="{ name: 'gebruikersgroepen' }">Gebruikersgroepen</router-link>
        </li>

        <li>
          <router-link :to="{ name: 'publicaties' }">Publicaties</router-link>
        </li>
      </ul>
    </nav>
  </header>
</template>

<script lang="ts" setup></script>

<style lang="scss" scoped></style>
