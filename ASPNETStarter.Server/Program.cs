using ASPNETStarter.Server.Application;
using ASPNETStarter.Server.Controllers;
using ASPNETStarter.Server.Extensions;
using ASPNETStarter.Server.Models;
using ASPNETStarter.Server.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ASPNETStarter.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthorization();
        builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options =>
        {
            // Make tokens short lived so that logout has an effect within a reasonable time frame (since tokens stay valid until they expire).
            options.BearerTokenExpiration = TimeSpan.FromMinutes(5);
            options.RefreshTokenExpiration = TimeSpan.FromDays(14);
        });

        // Configure ASP.NET Core Identity lockout options
        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        // Add services to the container.
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new RoutePrefixConvention(
                "ASPNETStarter.Server.Controllers.v1", "api/v1"));
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            // Register custom operation filter to only show lock for endpoints with [Authorize]
            c.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        builder.Services.AddTransient<SeederService>();

        var app = builder.Build();

        // Ensure database is created and migrations are applied
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                // This will create the database if it doesn't exist and apply any pending migrations
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
                // You might want to decide whether to continue or throw based on your requirements
                throw;
            }
        }

        // Seed the database with initial data
        await app.SeedDatabaseAsync();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapIdentityApiFilterable<ApplicationUser>();
        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}