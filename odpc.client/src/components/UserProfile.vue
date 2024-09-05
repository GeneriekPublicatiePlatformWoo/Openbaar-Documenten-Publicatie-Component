<template>
  <h1>User</h1>
  <div v-if="user.isLoggedIn">
    <dl>
      <template v-for="(value, key) in user" :key="key">
        <dt>{{ key }}</dt>
        <dd>{{ value }}</dd>
      </template>
    </dl>
    <a href="/api/logoff">Uitloggen</a>
  </div>
  <a href="/api/challenge" v-else>Inloggen</a>
</template>

<script lang="ts" setup>
import { fetchLoggedIn } from '@/api/fetch-logged-in'
const user = await fetchLoggedIn('/api/me').then((r) => r.json())
</script>

<style scoped>
dl {
  display: grid;
  grid-template-columns: 1fr 1fr;
}

div {
  text-align: left;
}
</style>
