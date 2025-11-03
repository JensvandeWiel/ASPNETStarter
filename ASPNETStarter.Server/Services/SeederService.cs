using System.Reflection;
using ASPNETStarter.Server.Application;
using ASPNETStarter.Server.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ASPNETStarter.Server.Services;

public class SeederService
{
    private readonly ILogger<SeederService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SeederService(IServiceProvider serviceProvider, ILogger<SeederService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task SeedAllAsync(ApplicationDbContext context)
    {
        var seederTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<SeederAttribute>() != null &&
                        typeof(ISeeder).IsAssignableFrom(t) &&
                        !t.IsInterface && !t.IsAbstract)
            .OrderBy(t => t.GetCustomAttribute<SeederAttribute>()!.Order)
            .ToList();

        foreach (var seederType in seederTypes)
        {
            _logger.LogInformation("Seeding with {SeederType}", seederType.Name);
            var seeder = (ISeeder)Activator.CreateInstance(seederType)!;
            
            var shouldRun = await seeder.ShouldRunAsync(context, _serviceProvider);
            if (!shouldRun)
            {
                _logger.LogInformation("Skipping seeder {SeederType} as it should not run", seederType.Name);
                continue;
            }
            await seeder.SeedAsync(context, _serviceProvider);
            var attr = seederType.GetCustomAttribute<SeederAttribute>();
            context.SeederHistories.Add(new Models.SeederHistory
            {
                SeederName = seederType.Name,
                LastSeededAt = DateTime.UtcNow,
                SeedPriority = attr!.Order
            });
            await context.SaveChangesAsync();
            _logger.LogInformation("Finished seeding with {SeederType}", seederType.Name);
        }
    }
}