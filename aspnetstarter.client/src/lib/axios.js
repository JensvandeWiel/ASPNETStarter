import axios from 'axios';
import {useAuthStore} from '@/stores/auth-store.js';
import {router} from '@/main.js';
import {NotificationType, useNotifyStore} from "@/stores/notify-store.js";

const api = axios.create();

/**
 * Request interceptor: add token to /api requests
 */
api.interceptors.request.use((config) => {
  const authStore = useAuthStore();
  if (config.url && (config.url.startsWith('/api') || config.url.startsWith('api') || config.url.startsWith('auth') || config.url.startsWith('/auth'))) {
    if (authStore.token) {
      config.headers = config.headers || {};
      config.headers['Authorization'] = `Bearer ${authStore.token}`;
    }
  }
  return config;
});

/**
 * Response interceptor: refresh token if expired
 */
api.interceptors.response.use(
    (response) => response,
    async (error) => {
      const authStore = useAuthStore();
      const notifyStore = useNotifyStore();
      const originalRequest = error.config;
      if (error.response && error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        try {
          await authStore.refresh();
          originalRequest.headers['Authorization'] = `Bearer ${authStore.token}`;
          return api(originalRequest);
        } catch (refreshError) {
          authStore.clearTokens();
          router.push('/login');
          notifyStore.notify("Sessie is verlopen, log opnieuw in", NotificationType.Error);
          return Promise.reject(new Error('Session expired. Please log in again.'));
        }
      }
      if (error.response && error.response.status === 401) {
        authStore.clearTokens();
      }
      return Promise.reject(error);
    }
);

export default api;

