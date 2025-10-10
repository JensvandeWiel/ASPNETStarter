using ASPNETStarter.Server.Application;
using Microsoft.EntityFrameworkCore;
using ASPNETStarter.Server.Services;

namespace ASPNETStarter.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var seederService = services.GetRequiredService<SeederService>();
            
            await seederService.SeedAllAsync(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }

        return app;
    }
}
