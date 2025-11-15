import {defineStore} from "pinia";
import axios, {AxiosError, type AxiosResponse} from "axios";
import type {
  AccessTokenResponse,
  LoginOptions,
  LoginRequestBody,
  RefreshRequestBody
} from "@/types/auth.ts";
import {$ResetPinia} from "@/lib/pinia.ts";

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || '',
    refreshToken: localStorage.getItem('refreshToken') || ''
  }),
  getters: {
    isAuthenticated: (state) => state.token.length > 0 && state.refreshToken.length > 0,
  },

  actions: {
    setToken(token: string) {
      this.token = token;
      localStorage.setItem('token', token);
    },
    setRefreshToken(refreshToken: string) {
      this.refreshToken = refreshToken;
      localStorage.setItem('refreshToken', refreshToken);
    },
    clearTokens() {
      this.token = '';
      this.refreshToken = '';
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
    },
    async login(email: string, password: string, loginOptions?: LoginOptions) {
      let loginBody: LoginRequestBody = {
        email,
        password
      };
      let response: AxiosResponse<AccessTokenResponse>;

      try {
        response = await axios<AccessTokenResponse>({
          method: 'post',
          url: 'auth/login',
          data: loginBody,
          params: loginOptions,
          responseType: 'json',

        });
      } catch (e) {
        if ((e as AxiosError).response?.status === 401) {
          throw new Error('Ongeldige inloggegevens');
        } else {
          throw e;
        }
      }

      this.setToken(response.data.accessToken);
      this.setRefreshToken(response.data.refreshToken);

      return response.data;
    },

    async logout(router: any) {
      let response: AxiosResponse | undefined = undefined;

      if (!this.isAuthenticated) {
        throw new Error('Niet ingelogd');
      }

      try {
        response = await axios<AccessTokenResponse>({
          method: 'post',
          url: 'auth/logout',
          headers: {
            'Authorization': `Bearer ${this.token}`
          },
          responseType: 'json',

        });
      } catch (e) {
        const error = e as AxiosError;
        if (error.response?.status === 401) {
          // Is already logged out
          response = error.response;
        } else {
          throw e;
        }
      }
      this.clearTokens();

      // Clear all stores
      // @ts-expect-error -- adding 'all' method dynamically
      $ResetPinia().all()

      await this.redirectToLogin(router);
      return response;
    },

    async redirectToLogin(router: any) {
      await router.push('/login');
    },

    async refresh() {

      let refreshToken = this.refreshToken;

      let refreshRequestBody: RefreshRequestBody = {
        refreshToken
      };
      let response: AxiosResponse<AccessTokenResponse>;

      try {
        response = await axios<AccessTokenResponse>({
          method: 'post',
          url: 'auth/refresh',
          data: refreshRequestBody,
          responseType: 'json',

        });
      } catch (e) {
        if ((e as AxiosError).response?.status === 401) {
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
