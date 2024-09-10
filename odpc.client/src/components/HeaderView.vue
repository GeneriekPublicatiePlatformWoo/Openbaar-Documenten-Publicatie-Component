<template>
  <header>
    <h1>Openbare Documenten Publicatie Component</h1>

    <nav>
      <ul>
        <li>
          <router-link :to="{ name: 'home' }">Home</router-link>
        </li>

        <!-- <li v-if="user?.isLoggedIn"> -->
        <li>
          <router-link :to="{ name: 'publicaties' }">Publicaties</router-link>
        </li>

        <li>
          <router-link :to="{ name: 'gebruikersgroepen' }">Gebruikersgroepen</router-link>
        </li>

        <li v-if="user?.isLoggedIn">
          <a href="/api/logoff" title="Uitloggen">Uitloggen</a>
        </li>

        <li v-else>
          <a href="/api/challenge" title="Inloggen">Inloggen</a>
        </li>
      </ul>
    </nav>
  </header>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import getUser, { type User } from "@/stores/user";

const user = ref<User | null>(await getUser());
</script>

<style lang="scss" scoped></style>
