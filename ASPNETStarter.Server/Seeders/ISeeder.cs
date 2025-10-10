using Microsoft.EntityFrameworkCore;

namespace ASPNETStarter.Server.Services;

public interface ISeeder
{
    Task SeedAsync(DbContext context, IServiceProvider serviceProvider);
}