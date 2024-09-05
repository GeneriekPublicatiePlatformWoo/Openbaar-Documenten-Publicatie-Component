<template>
  <div class="weather-component">
    <Suspense>
      <UserProfile></UserProfile>
      <template #fallback>
        <div class="loading">
          Loading... Please refresh once the ASP.NET backend has started. See
          <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more
          details.
        </div>
      </template>
    </Suspense>
  </div>
</template>

<script lang="ts" setup>
import UserProfile from './UserProfile.vue'
</script>
