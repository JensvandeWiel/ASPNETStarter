# ASPNETStarter

A modern full-stack web application starter template built with ASP.NET Core 8.0 and Vue.js 3, featuring TypeScript, Vite, npm, Entity Framework Core, and ASP.NET Core Identity for authentication and authorization.

## 🏗️ Architecture

- **Backend**: ASP.NET Core 8.0 Web API with versioned, filterable endpoints
- **Frontend**: Vue.js 3 (Composition API) with TypeScript and Vite
- **Database**: SQL Server via Entity Framework Core 8.0
- **Authentication**: ASP.NET Core Identity with filterable endpoints and role-based authorization
- **Package Manager**: npm with Node.js
- **Containerization**: Docker with multi-stage builds

## 📁 Project Structure

```
ASPNETStarter/
├── ASPNETStarter.Server/          # Backend API (.NET 8.0)
│   ├── Application/               # EF Core context and roles
│   │   ├── ApplicationDbContext.cs
│   │   └── ApplicationRoles.cs
│   ├── Controllers/               # API controllers (versioned)
│   │   ├── v1/
│   │   │   └── WeatherForecastController.cs
│   │   └── RoutePrefixConvention.cs
│   ├── Extensions/                # Extension methods (e.g., seeding, routing)
│   │   ├── ApplicationBuilderExtensions.cs
│   │   ├── AuthorizeCheckOperationFilter.cs
│   │   ├── IdentityApiEndpointRouteBuilderExtensions.cs
│   │   └── IdentityApiEndpointRouteBuilderOptions.cs
│   ├── Migrations/                # EF Core migrations
│   │   ├── 20251010182208_AddAuthTables.cs
│   │   ├── 20251010182208_AddAuthTables.Designer.cs
│   │   └── ApplicationDbContextModelSnapshot.cs
│   ├── Models/                    # Domain models
│   │   └── ApplicationUser.cs
│   ├── Seeders/                   # Database seeding logic
│   │   ├── ISeeder.cs
│   │   ├── RoleSeeder.cs
│   │   └── SeederAttribute.cs
│   ├── Services/                  # Service classes
│   │   └── SeederService.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Program.cs                 # Application entry point
│   ├── Dockerfile                 # Docker configuration
│   └── ASPNETStarter.Server.csproj
│   └── appsettings.json           # Main configuration
│   └── appsettings.Development.json
│   └── docker-compose.yml         # Docker Compose config
├── aspnetstarter.client/          # Frontend SPA (Vue.js 3 + TypeScript)
│   ├── src/
│   │   ├── assets/                # Static assets (CSS, images)
│   │   ├── components/            # Vue components
│   │   ├── types/                 # TypeScript types
│   │   ├── App.vue                # Root component
│   │   ├── main.ts                # Application entry point
│   │   └── shims-vue.d.ts         # TypeScript shims
│   ├── public/                    # Public static files (favicon, etc.)
│   ├── vite.config.ts             # Vite configuration
│   ├── package.json               # Node/npm dependencies
│   ├── package-lock.json          # npm lockfile
│   ├── tsconfig.json              # TypeScript config
│   ├── tsconfig.app.json
│   ├── tsconfig.node.json
│   └── aspnetstarter.client.esproj
├── ASPNETStarter.sln              # Solution file
├── rename-project.ps1             # Project renaming utility
├── LICENSE                        # License file
└── README.md                      # Project documentation
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v20.19.0+ or v22.12.0+)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or full version)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional)

### Installation

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd ASPNETStarter
   ```

2. **Configure the database connection**
   Edit `ASPNETStarter.Server/appsettings.json` with your SQL Server connection string.

3. **Restore .NET dependencies**
   ```bash
   dotnet restore
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update --project ASPNETStarter.Server
   ```
   Or simply run the backend; migrations and seeding are automatic on startup.

5. **Install frontend dependencies**
   ```bash
   cd aspnetstarter.client
   npm install
   cd ..
   ```

### Running the Application

#### Development Mode

```bash
dotnet run --project ASPNETStarter.Server
```
- Starts backend on `https://localhost:5001`
- Applies migrations and seeds roles/users
- Starts Vite dev server for frontend with HMR
- Proxies API requests from frontend to backend

#### Manual Frontend Development

```bash
cd aspnetstarter.client
npm run dev
```

#### Production Build

```bash
dotnet build -c Release
dotnet publish -c Release -o ./publish
```

## 🗄️ Database Integration

- Uses **Entity Framework Core 8.0**
- Automatic migrations and seeding on startup
- Connection string in `appsettings.json`

**Manual migration commands:**
```bash
dotnet ef migrations add MigrationName --project ASPNETStarter.Server
dotnet ef database update --project ASPNETStarter.Server
dotnet ef database drop --project ASPNETStarter.Server # (development only)
```

## 🔐 Authentication & Authorization

### Identity & Filterable Endpoints

Authentication uses ASP.NET Core Identity, but endpoints are exposed via `MapIdentityApiFilterable`. This provides the same endpoints as standard Identity, but allows filtering, customization, and extension. You can secure, extend, or filter endpoints as needed, while maintaining compatibility with ASP.NET conventions.

**Available endpoints include:**
- `/register`, `/login`, `/logout`, `/refresh`, `/confirmEmail`, `/resendConfirmationEmail`, `/forgotPassword`, `/resetPassword`, `/manage/2fa`, `/manage/info`

### Logout, Security Stamp, and Token Lifetime

- **Logout**: When a user logs out via the `/logout` endpoint, the backend immediately updates the user's security stamp. This action invalidates all existing bearer tokens for that user, ensuring that any previously issued tokens cannot be used for further access.
- **Security Stamp**: The security stamp is a unique value associated with each user. It is checked during token refresh and protected endpoint access. If the security stamp has changed (due to logout, password change, etc.), any old tokens are rejected with a 401 Unauthorized response.
- **Short-Lived Tokens**: Tokens are intentionally configured to be short-lived. This means that even if a token is not immediately invalidated by logout, it will expire quickly, minimizing the risk window for unauthorized access.
- **Practical Effect**: Logging out is effective and secure. After logout, all tokens are invalidated, and any remaining tokens expire soon. This prevents continued access after logout, even if a token is leaked or reused.
- **Best Practices**: This approach follows modern security best practices, leveraging ASP.NET Core Identity’s built-in mechanisms for security stamp validation and token management.

**Usage example:**
```csharp
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase { }
```

## 📑 API Definitions & Swagger

- Uses Swashbuckle for OpenAPI/Swagger documentation
- Custom operation filter marks `[Authorize]` endpoints in Swagger UI
- Interactive API docs at `https://localhost:5001/swagger`
- Bearer authentication supported in Swagger UI

**How to test protected endpoints:**
1. Register/Login via Identity endpoints
2. Copy JWT access token
3. Click "Authorize" in Swagger UI, paste token
4. Test endpoints

## 🌱 Database Seeding

- Automatic discovery and execution of seeders via reflection
- Seeders implement `ISeeder` and are marked with `[Seeder(order: X)]`
- Built-in seeders: `RoleSeeder` (roles), custom seeders supported
- Runs on startup, idempotent

**Example custom seeder:**
```csharp
[Seeder(order: 2)]
public class UserSeeder : ISeeder {
    public async Task SeedAsync(DbContext context, IServiceProvider serviceProvider) {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        if (!await userManager.Users.AnyAsync()) {
            var adminUser = new ApplicationUser {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
```

## 🐳 Docker Support

- Multi-stage Dockerfile for optimized builds
- Installs Node.js, builds frontend/backend, publishes minimal runtime image

**Build and run:**
```bash
docker build -f ASPNETStarter.Server/Dockerfile -t aspnetstarter:latest .
docker run -d -p 8080:8080 --name aspnetstarter aspnetstarter:latest
```
App available at `http://localhost:8080`

## 🔌 API Versioning

- Namespace-based versioning via `RoutePrefixConvention`
- Controllers in `Controllers.v1` → `/api/v1`, add `Controllers.v2` for `/api/v2`

## 🎨 Frontend

- **Vue.js 3** (Composition API)
- **TypeScript**
- **Vite 7.x**
- **Node.js** with npm
- **ESLint** for linting
- **Hot Module Replacement**
- **Path Aliases** (`@/` for `src`)
- **API Proxy** for seamless backend integration

**Frontend scripts:**
```bash
npm run dev      # Start dev server
npm run type-check # Type-check TypeScript
npm run build    # Build for production
npm run preview  # Preview production build
npm run lint     # Lint and fix code
```

## ⚙️ Configuration

- **Backend**: `appsettings.json`, `launchSettings.json` for connection strings, logging, CORS, Identity options
- **Frontend**: `vite.config.ts` for dev server, proxy, build options
- **package.json** for dependencies and scripts

## 🔄 Renaming the Project

Use the provided PowerShell script to rename the project:
```powershell
./rename-project.ps1 -NewProjectName "YourProjectName"
```
- Updates all file contents, renames files/folders, cleans build artifacts
- After renaming:
```bash
dotnet restore
cd yourprojectname.client
npm install
cd ..
dotnet build
```

## 📦 Dependencies

- **Backend**: Microsoft.AspNetCore.Identity.EntityFrameworkCore, EntityFrameworkCore.SqlServer, Swashbuckle.AspNetCore, etc.
- **Frontend**: vue, vite, typescript, @vitejs/plugin-vue, eslint, vue-tsc

## 📝 License

MIT License. See `LICENSE` for details.

---

**Built with ❤️ using ASP.NET Core, Vue.js, and Entity Framework Core**
