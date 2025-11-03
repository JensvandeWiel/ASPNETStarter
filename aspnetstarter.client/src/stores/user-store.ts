import {defineStore} from "pinia";
import api from "@/lib/axios";
import type {User} from "@/types/user";

interface State {
  user: User | null;
  loading: boolean;
  error: string | null;
  fetchPromise: Promise<User | null> | null;
}

export const useUserStore = defineStore('user', {
  state: (): State => ({
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
    setUser(user: User | null) {
      this.user = user;
      this.error = null;
    },

    clearUser() {
      this.user = null;
      this.error = null;
      this.fetchPromise = null;
    },

    async getUser(): Promise<User | null> {
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

    async fetchUser(): Promise<User | null> {
      this.loading = true;
      this.error = null;

      const response = await api.get<User>('/auth/manage/info');

      this.user = response.data;
      this.loading = false;
      return this.user;
    },

    refetchUser(): Promise<User | null> {
      this.fetchPromise = null;
      this.user = null;
      return this.getUser();
    }
  }
});
