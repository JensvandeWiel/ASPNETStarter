import path from 'node:path';
import {defineConfig} from 'vite';
import plugin from '@vitejs/plugin-vue';
import {env} from 'node:process';
import tailwindcss from '@tailwindcss/vite'


/**
 * Determines the target URL for the development server proxy
 * @returns {string} The target URL for proxying API requests
 */
function getProxyTarget() {
  // Check for HTTPS port first
  if (env.ASPNETCORE_HTTPS_PORT !== undefined && env.ASPNETCORE_HTTPS_PORT.length > 0) {
    return `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`;
  }

  // Check for configured URLs
  if (env.ASPNETCORE_URLS !== undefined && env.ASPNETCORE_URLS.length > 0) {
    const urls = env.ASPNETCORE_URLS.split(';');
    const firstUrl = urls[0];
    if (firstUrl.length > 0) {
      return firstUrl;
    }
  }

  // Default fallback
  return 'http://localhost:5239';
}

const target = getProxyTarget();

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [plugin(), tailwindcss()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    proxy: {
      '^/api/.*': {
        target,
        secure: false
      },
      '^/auth/.*': {
        target,
        secure: false
      },
    },
    port: parseInt(env.DEV_SERVER_PORT ?? '3000'),
    host: '127.0.0.1'
  }
})

