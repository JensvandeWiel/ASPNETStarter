using ASPNETStarter.Server.Application;
using Microsoft.EntityFrameworkCore;

namespace ASPNETStarter.Server.Services;

public interface ISeeder
{
    Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider);
    Task<bool> ShouldRunAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        var type = GetType();
        return Task.FromResult(!context.SeederHistories.AnyAsync(sh => sh.SeederName == type.Name).Result);
    }
}