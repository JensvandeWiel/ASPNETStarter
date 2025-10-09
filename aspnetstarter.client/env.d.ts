/// <reference types="vite/client" />

declare namespace NodeJS {
  interface ProcessEnv {
    // ASP.NET Core environment variables
    ASPNETCORE_HTTPS_PORT?: string | null;
    ASPNETCORE_URLS?: string | null;

    // Development server configuration
    DEV_SERVER_PORT?: string | null;
  }
}
