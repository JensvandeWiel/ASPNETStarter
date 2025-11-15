import {defineStore} from "pinia";
import axios from "axios";
import {$ResetPinia} from "@/lib/pinia.js";

/**
 * @typedef {Object} State
 * @property {string} token
 * @property {string} refreshToken
 */

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || '',
    refreshToken: localStorage.getItem('refreshToken') || ''
  }),
  getters: {
    isAuthenticated: (state) => state.token.length > 0 && state.refreshToken.length > 0,
  },

  actions: {
    /**
     * Set the access token
     * @param {string} token
     */
    setToken(token) {
      this.token = token;
      localStorage.setItem('token', token);
    },
    /**
     * Set the refresh token
     * @param {string} refreshToken
     */
    setRefreshToken(refreshToken) {
      this.refreshToken = refreshToken;
      localStorage.setItem('refreshToken', refreshToken);
    },
    /**
     * Clear both tokens
     */
    clearTokens() {
      this.token = '';
      this.refreshToken = '';
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
    },
    /**
     * Login with email and password
     * @param {string} email
     * @param {string} password
     * @param {any} [loginOptions]
     * @returns {Promise<any>}
     */
    async login(email, password, loginOptions) {
      const loginBody = {
        email,
        password
      };

      let response;

      try {
        response = await axios({
          method: 'post',
          url: 'auth/login',
          data: loginBody,
          params: loginOptions,
          responseType: 'json',
        });
      } catch (e) {
        if (e.response?.status === 401) {
          throw new Error('Ongeldige inloggegevens');
        } else {
          throw e;
        }
      }

      this.setToken(response.data.accessToken);
      this.setRefreshToken(response.data.refreshToken);

      return response.data;
    },

    /**
     * Logout the user
     * @param {any} router
     * @returns {Promise<any>}
     */
    async logout(router) {
      let response = undefined;

      if (!this.isAuthenticated) {
        throw new Error('Niet ingelogd');
      }

      try {
        response = await axios({
          method: 'post',
          url: 'auth/logout',
          headers: {
            'Authorization': `Bearer ${this.token}`
          },
          responseType: 'json',
        });
      } catch (e) {
        const error = e;
        if (error.response?.status === 401) {
          // Is already logged out
          response = error.response;
        } else {
          throw e;
        }
      }
      this.clearTokens();

      // Clear all stores
      $ResetPinia().all()

      await this.redirectToLogin(router);
      return response;
    },

    /**
     * Redirect to login page
     * @param {any} router
     * @returns {Promise<void>}
     */
    async redirectToLogin(router) {
      await router.push('/login');
    },

    /**
     * Refresh the access token
     * @returns {Promise<any>}
     */
    async refresh() {
      const refreshToken = this.refreshToken;

      const refreshRequestBody = {
        refreshToken
      };

      let response;

      try {
        response = await axios({
          method: 'post',
          url: 'auth/refresh',
          data: refreshRequestBody,
          responseType: 'json',
        });
      } catch (e) {
        if (e.response?.status === 401) {
          throw new Error('Niet ingelogd of sessie verlopen');
        } else {
          throw e;
        }
      }

      this.setToken(response.data.accessToken);
      this.setRefreshToken(response.data.refreshToken);

      return response.data;
    }
  }
});

