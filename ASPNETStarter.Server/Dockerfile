# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-bun
RUN apt-get update
RUN apt-get install -y curl unzip
RUN curl -fsSL https://bun.sh/install | bash
ENV PATH="/root/.bun/bin:$PATH"
# Create symbolic link so npm commands use bun
RUN ln -s /root/.bun/bin/bun /usr/local/bin/npm


FROM with-bun AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ASPNETStarter.Server/ASPNETStarter.Server.csproj", "ASPNETStarter.Server/"]
COPY ["aspnetstarter.client/aspnetstarter.client.esproj", "aspnetstarter.client/"]
COPY ["aspnetstarter.client/package.json", "aspnetstarter.client/"]
COPY ["aspnetstarter.client/package-lock.json", "aspnetstarter.client/"]
RUN dotnet restore "./ASPNETStarter.Server/ASPNETStarter.Server.csproj"
# Install JavaScript dependencies with Bun for the correct platform
WORKDIR /src/aspnetstarter.client
RUN bun install
WORKDIR /src
COPY . .
WORKDIR "/src/ASPNETStarter.Server"
RUN dotnet build "./ASPNETStarter.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ASPNETStarter.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASPNETStarter.Server.dll"]
