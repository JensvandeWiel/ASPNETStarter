# ASPNETStarter

A modern full-stack web application starter template built with ASP.NET Core 8.0 and Vue.js 3, featuring TypeScript, Vite, Bun, Entity Framework Core, and ASP.NET Core Identity for authentication.

## 🏗️ Architecture

This solution follows a client-server architecture with:

- **Backend**: ASP.NET Core 8.0 Web API with versioned endpoints
- **Frontend**: Vue.js 3 with TypeScript and Vite
- **Database**: SQL Server with Entity Framework Core 8.0
- **Authentication**: ASP.NET Core Identity with role-based authorization
- **Package Manager**: Bun (for faster dependency management)
- **Containerization**: Docker support with optimized multi-stage builds

## 📁 Project Structure

```
ASPNETStarter/
├── ASPNETStarter.Server/          # Backend API (.NET 8.0)
│   ├── Application/
│   │   ├── ApplicationDbContext.cs      # EF Core DbContext
│   │   └── ApplicationRoles.cs          # Role definitions enum
│   ├── Controllers/
│   │   ├── v1/                    # Versioned API controllers
│   │   │   └── WeatherForecastController.cs
│   │   └── RoutePrefixConvention.cs
│   ├── Extensions/
│   │   └── ApplicationBuilderExtensions.cs  # Seeding extensions
│   ├── Migrations/                # EF Core migrations
│   │   └── 20251010182208_AddAuthTables.cs
│   ├── Models/
│   │   └── ApplicationUser.cs     # Custom Identity user
│   ├── Seeders/                   # Database seeding
│   │   ├── ISeeder.cs
│   │   ├── RoleSeeder.cs
│   │   └── SeederAttribute.cs
│   ├── Services/
│   │   └── SeederService.cs       # Automatic seeder discovery
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
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or full version)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for containerization)

### Installation

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd ASPNETStarter
   ```

2. **Configure the database connection**
   
   Update the connection string in `ASPNETStarter.Server/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ASPNETStarterDB;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

3. **Restore .NET dependencies**
   ```bash
   dotnet restore
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update --project ASPNETStarter.Server
   ```
   
   Or simply run the application - it will automatically create the database and apply migrations on startup.

5. **Install frontend dependencies**
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
- Automatically create/migrate the database
- Seed initial data (roles)
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

## 🗄️ Database Integration

### Entity Framework Core

The application uses **Entity Framework Core 8.0** with **SQL Server** as the database provider.

**Key Components:**
- **ApplicationDbContext**: Main database context inheriting from `IdentityDbContext<ApplicationUser>`
- **Automatic Migrations**: Database is created and migrations are applied automatically on application startup
- **Connection String**: Configured in `appsettings.json` under `ConnectionStrings:DefaultConnection`

### Database Management

**Apply migrations manually:**
```bash
dotnet ef migrations add MigrationName --project ASPNETStarter.Server
dotnet ef database update --project ASPNETStarter.Server
```

**Drop database (development only):**
```bash
dotnet ef database drop --project ASPNETStarter.Server
```

## 🔐 Authentication & Authorization

### ASP.NET Core Identity

The application includes a complete authentication system using **Microsoft.AspNetCore.Identity.EntityFrameworkCore**:

**Features:**
- ✅ User registration and login
- ✅ Identity API endpoints (`/register`, `/login`, `/logout`, etc.)
- ✅ Role-based authorization
- ✅ JWT Bearer token support
- ✅ Swagger integration with Bearer authentication
- ✅ Custom `ApplicationUser` model (extends `IdentityUser`)

### Identity API Endpoints

The following endpoints are automatically available:

- `POST /register` - Register a new user
- `POST /login` - Login with email and password
- `POST /refresh` - Refresh access token
- `POST /logout` - Logout current user
- `GET /confirmEmail` - Confirm email address
- `POST /resendConfirmationEmail` - Resend confirmation email
- `POST /forgotPassword` - Request password reset
- `POST /resetPassword` - Reset password
- `POST /manage/2fa` - Two-factor authentication management
- `GET /manage/info` - Get user information
- `POST /manage/info` - Update user information

### Predefined Roles

The application includes three predefined roles defined in the `ApplicationRoles` enum:

1. **Admin** - Full system access and administrative privileges
2. **User** - Standard user access (default role for new users)
3. **Moderator** - Elevated permissions for content moderation

These roles are automatically seeded into the database on application startup.

### Using Roles in Controllers

```csharp
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    // Only accessible to Admin users
}

[Authorize(Roles = "Admin,Moderator")]
public IActionResult ModerateContent()
{
    // Accessible to both Admin and Moderator
}
```

## 🌱 Database Seeding

### Seeder System

The application features a sophisticated automatic database seeding system that:

- ✅ Discovers seeders automatically via reflection
- ✅ Executes seeders in a specified order
- ✅ Runs on application startup
- ✅ Logs seeding operations
- ✅ Extensible for custom seeders

### Architecture

**Key Components:**
- **ISeeder Interface**: Contract for all seeders
- **SeederAttribute**: Marks classes as seeders with execution order
- **SeederService**: Discovers and executes all seeders
- **ApplicationBuilderExtensions**: Extension method for easy integration

### Creating Custom Seeders

1. Create a new class implementing `ISeeder`
2. Add the `[Seeder(order: X)]` attribute
3. Implement the `SeedAsync` method

**Example:**

```csharp
[Seeder(order: 2)]
public class UserSeeder : ISeeder
{
    public async Task SeedAsync(DbContext context, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        if (!await userManager.Users.AnyAsync())
        {
            var adminUser = new ApplicationUser
            {
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

### Built-in Seeders

**RoleSeeder** (order: 1)
- Seeds all predefined roles from the `ApplicationRoles` enum
- Ensures roles exist before any user creation
- Idempotent - safe to run multiple times

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
- **Bearer Authentication**: Click "Authorize" button and enter your JWT token
- Test endpoints directly from the browser
- Includes Identity API endpoints and custom controllers

### Authentication in Swagger

1. Register/Login using Identity endpoints
2. Copy the received access token
3. Click the "Authorize" button in Swagger UI
4. Enter: `Bearer <your-token>` (or just the token)
5. Test protected endpoints

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
- **ConnectionStrings:DefaultConnection**: Database connection string
- Configure logging levels
- CORS policies
- Identity options (password requirements, lockout settings, etc.)
- Custom application settings

**launchSettings.json**
- Development server ports
- Environment variables
- SSL configuration

**Example appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ASPNETStarterDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

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
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore** (8.0.20) - Identity system with EF Core
- **Microsoft.EntityFrameworkCore.SqlServer** (8.0.20) - SQL Server database provider
- **Microsoft.EntityFrameworkCore.Tools** (8.0.20) - EF Core migrations and tooling
- **Swashbuckle.AspNetCore** (6.6.2) - Swagger/OpenAPI documentation
- **Microsoft.VisualStudio.Web.CodeGeneration.Design** (8.0.7) - Code generation tools

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

**Built with ❤️ using ASP.NET Core, Vue.js, and Entity Framework Core**
