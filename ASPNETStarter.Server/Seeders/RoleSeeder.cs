using ASPNETStarter.Server.Application;
using ASPNETStarter.Server.Services;
using Microsoft.AspNetCore.Identity;

namespace ASPNETStarter.Server.Seeders;

[Seeder(1)]
public class RoleSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = Enum.GetValues<ApplicationRoles>();

        foreach (var role in roles)
        {
            var roleName = role.ToString();
            var identityRole = new IdentityRole(roleName);
            await roleManager.CreateAsync(identityRole);
        }
    }
}