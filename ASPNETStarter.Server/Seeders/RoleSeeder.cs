using ASPNETStarter.Server.Application;
using ASPNETStarter.Server.Models;
using ASPNETStarter.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASPNETStarter.Server.Seeders;

[Seeder(order: 1)]
public class RoleSeeder : ISeeder
{
    public async Task SeedAsync(DbContext context, IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        var roles = Enum.GetValues<ApplicationRoles>();

        foreach (var role in roles)
        {
            var roleName = role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}