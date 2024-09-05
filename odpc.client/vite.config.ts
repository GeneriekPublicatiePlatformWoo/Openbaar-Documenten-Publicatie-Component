import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import mockServer from 'vite-plugin-mock-server'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), mockServer()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  }
})
