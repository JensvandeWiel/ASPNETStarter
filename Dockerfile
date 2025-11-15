# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js and npm
RUN apt-get update && apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_24.x | bash - && \
    apt-get install -y nodejs && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY . .
RUN dotnet restore "./ASPNETStarter.Server/ASPNETStarter.Server.csproj"
# Install JavaScript dependencies with npm for the correct platform
WORKDIR /src/aspnetstarter.client
RUN npm install
RUN npm run build

WORKDIR /src

# Clean up conflicting obj files before build
RUN find ./ASPNETStarter.Server/obj -name "*sync-conflict*" -delete
WORKDIR "/src/ASPNETStarter.Server"
RUN dotnet build "./ASPNETStarter.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build -p:SpaRoot="../aspnetstarter.client" -p:SpaProxyServerUrl="http://localhost:3000"

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ASPNETStarter.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASPNETStarter.Server.dll"]
