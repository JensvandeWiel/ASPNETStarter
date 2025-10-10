using System.Reflection;
using ASPNETStarter.Server.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ASPNETStarter.Server.Services;

public class SeederService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeederService> _logger;

    public SeederService(IServiceProvider serviceProvider, ILogger<SeederService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task SeedAllAsync(DbContext context)
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
            await seeder.SeedAsync(context, _serviceProvider);
            _logger.LogInformation("Finished seeding with {SeederType}", seederType.Name);
        }
    }
}