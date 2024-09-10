import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import mockServer from 'vite-plugin-mock-server'

const proxyCalls = ['/api', '/signin-oidc', '/signout-callback-oidc', '/healthz']

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), mockServer({ urlPrefixes: [ '/api-mock/' ] })],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    proxy: Object.fromEntries(
      proxyCalls.map((key) => [
        key,
        {
          target: 'http://localhost:5252',
          secure: false
        }
      ])
    )
  },
  build: {
    assetsInlineLimit: 0
  }
})
