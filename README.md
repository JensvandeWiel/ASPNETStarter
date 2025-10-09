# ASPNETStarter

A modern full-stack web application starter template built with ASP.NET Core 8.0 and Vue.js 3, featuring TypeScript, Vite, and Bun.

## 🏗️ Architecture

This solution follows a client-server architecture with:

- **Backend**: ASP.NET Core 8.0 Web API with versioned endpoints
- **Frontend**: Vue.js 3 with TypeScript and Vite
- **Package Manager**: Bun (for faster dependency management)
- **Containerization**: Docker support with optimized multi-stage builds

## 📁 Project Structure

```
ASPNETStarter/
├── ASPNETStarter.Server/          # Backend API (.NET 8.0)
│   ├── Controllers/
│   │   ├── v1/                    # Versioned API controllers
│   │   │   └── WeatherForecastController.cs
│   │   └── RoutePrefixConvention.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Program.cs                 # Application entry point
│   ├── Dockerfile                 # Docker configuration
│   └── ASPNETStarter.Server.csproj
│
├── aspnetstarter.client/          # Frontend SPA (Vue.js 3 + TypeScript)
│   ├── src/
│   │   ├── assets/               # Static assets (CSS, images)
│   │   ├── components/           # Vue components
│   │   ├── App.vue               # Root component
│   │   └── main.ts               # Application entry point
│   ├── public/                   # Public static files
│   ├── vite.config.ts            # Vite configuration
│   ├── package.json              # Node dependencies
│   └── aspnetstarter.client.esproj
│
├── ASPNETStarter.sln             # Solution file
└── rename-project.ps1            # Project renaming utility
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Bun](https://bun.sh/) (v1.0.0 or higher) - Fast JavaScript runtime
- [Node.js](https://nodejs.org/) (v20.19.0+ or v22.12.0+) - Alternative to Bun
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerization)

### Installation

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd ASPNETStarter
   ```

2. **Restore .NET dependencies**
   ```bash
   dotnet restore
   ```

3. **Install frontend dependencies**
   ```bash
   cd aspnetstarter.client
   bun install
   cd ..
   ```

### Running the Application

#### Development Mode (with Hot Reload)

The simplest way to run both the backend and frontend:

```bash
dotnet run --project ASPNETStarter.Server
```

This will:
- Start the ASP.NET Core backend on `https://localhost:5001` (or configured port)
- Automatically start the Vite dev server on `https://localhost:57409`
- Enable hot module replacement (HMR) for frontend changes
- Proxy API requests from frontend to backend

#### Manual Frontend Development

To run the frontend separately:

```bash
cd aspnetstarter.client
bun run dev
```

#### Production Build

```bash
dotnet build -c Release
dotnet publish -c Release -o ./publish
```

## 🐳 Docker Support

### Building the Docker Image

```bash
docker build -f ASPNETStarter.Server/Dockerfile -t aspnetstarter:latest .
```

### Running the Container

```bash
docker run -d -p 8080:8080 --name aspnetstarter aspnetstarter:latest
```

The application will be available at `http://localhost:8080`

### Docker Architecture

The Dockerfile uses a multi-stage build process:

1. **with-bun stage**: Installs Node.js v24 and Bun in the SDK image
2. **build stage**: Restores dependencies and builds both frontend and backend
3. **publish stage**: Creates optimized production build
4. **final stage**: Minimal runtime image with only necessary files

## 🔌 API

### API Versioning

This project uses namespace-based API versioning with the `RoutePrefixConvention`. Controllers in different namespaces can have different route prefixes:

- `ASPNETStarter.Server.Controllers.v1` → `/api/v1`
- Add `ASPNETStarter.Server.Controllers.v2` → `/api/v2` for future versions

### Swagger/OpenAPI

Swagger UI is available in development mode:
- URL: `https://localhost:5001/swagger`
- Provides interactive API documentation
- Test endpoints directly from the browser

## 🎨 Frontend

### Technology Stack

- **Framework**: Vue.js 3 (Composition API)
- **Language**: TypeScript
- **Build Tool**: Vite 7.x
- **Styling**: CSS3 with scoped styles
- **Linting**: ESLint 9.x with Vue plugin

### Key Features

- **Hot Module Replacement (HMR)**: Instant updates during development
- **TypeScript Support**: Full type safety
- **Component-based Architecture**: Reusable Vue components
- **Path Aliases**: Use `@/` to reference the `src` directory
- **API Proxy**: Seamless backend integration during development

### Development Scripts

```bash
# Start dev server with HMR
bun run dev

# Type-check TypeScript
bun run type-check

# Build for production
bun run build

# Preview production build
bun run preview

# Lint and fix code
bun run lint
```

## ⚙️ Configuration

### Backend Configuration

**appsettings.json** / **appsettings.Development.json**
- Configure logging levels
- Database connection strings
- CORS policies
- Custom application settings

**launchSettings.json**
- Development server ports
- Environment variables
- SSL configuration

### Frontend Configuration

**vite.config.ts**
- Development server port (default: 57409)
- API proxy configuration
- Build optimizations
- Path aliases

**package.json**
- Dependencies and dev dependencies
- Script commands
- Engine requirements (Bun/Node versions)

## 🔄 Renaming the Project

A PowerShell script is provided to rename the entire project:

```powershell
# Windows (PowerShell)
.\rename-project.ps1 -NewProjectName "YourProjectName"
```

This script will:
1. Update all file contents (solution, projects, source files)
2. Rename files containing "ASPNETStarter" or "aspnetstarter"
3. Rename directories
4. Clean build artifacts (bin, obj, dist, node_modules)
5. Update the solution file

After renaming:
```bash
dotnet restore
cd yourprojectname.client
bun install
cd ..
dotnet build
```

## 📦 Dependencies

### Backend

- **Microsoft.AspNetCore.SpaProxy** (8.x) - SPA development server integration
- **Swashbuckle.AspNetCore** (6.6.2) - Swagger/OpenAPI documentation

### Frontend

- **vue** (3.5.22) - Progressive JavaScript framework
- **vite** (7.1.7) - Next generation frontend tooling
- **typescript** (5.9.0) - Typed JavaScript
- **@vitejs/plugin-vue** (6.0.1) - Vue plugin for Vite
- **eslint** (9.33.0) - Code linting
- **vue-tsc** (3.1.0) - TypeScript compiler for Vue

## 📝 License

MIT License. See `LICENSE` file for details.


---

**Built with ❤️ using ASP.NET Core and Vue.js**

