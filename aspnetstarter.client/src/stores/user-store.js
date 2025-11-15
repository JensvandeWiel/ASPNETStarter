import {defineStore} from "pinia";
import api from "@/lib/axios.js";

/**
 * @typedef {Object} State
 * @property {any} user
 * @property {boolean} loading
 * @property {string|null} error
 * @property {Promise|null} fetchPromise
 */

export const useUserStore = defineStore('user', {
  state: () => ({
    user: null,
    loading: false,
    error: null,
    fetchPromise: null,
  }),

  getters: {
    isLoaded: (state) => state.user !== null,
    currentUser: (state) => state.user,
    hasError: (state) => state.error !== null,
  },

  actions: {
    /**
     * Set the current user
     * @param {any} user
     */
    setUser(user) {
      this.user = user;
      this.error = null;
    },

    /**
     * Clear the current user
     */
    clearUser() {
      this.user = null;
      this.error = null;
      this.fetchPromise = null;
    },

    /**
     * Get the current user, fetching if needed
     * @returns {Promise<any>}
     */
    async getUser() {
      // If already loaded, return it
      if (this.user !== null) {
        return this.user;
      }

      // If already fetching, reuse the same promise
      if (this.fetchPromise) {
        return this.fetchPromise;
      }

      // Start a new fetch
      this.fetchPromise = this.fetchUser();
      return this.fetchPromise;
    },

    /**
     * Fetch the user from the server
     * @returns {Promise<any>}
     */
    async fetchUser() {
      this.loading = true;
      this.error = null;

      const response = await api.get('/auth/manage/info');

      this.user = response.data;
      this.loading = false;
      return this.user;
    },

    /**
     * Refetch the user
     * @returns {Promise<any>}
     */
    refetchUser() {
      this.fetchPromise = null;
      this.user = null;
      return this.getUser();
    }
  }
});

