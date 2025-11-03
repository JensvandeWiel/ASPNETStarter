export type LoginRequestBody = {
  email: string;
  password: string;
}

export type LoginOptions = {
  useCookies?: boolean;
  useSessionCookies?: boolean;
}

export type AccessTokenResponse = {
  tokenType: string;
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export type RefreshRequestBody = {
  refreshToken: string;
}
