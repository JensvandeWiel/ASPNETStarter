using System.Reflection;
using ASPNETStarter.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASPNETStarter.Server.Application;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    public DbSet<SeederHistory> SeederHistories => Set<SeederHistory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Get all types that implement ConfigurableModel
        var configurableModelTypes = typeof(ApplicationDbContext).Assembly.GetTypes()
            .Where(t => typeof(ConfigurableModel)
                .IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Where(t => t.Namespace == "ASPNETStarter.Server.Models");

        // Invoke OnModelCreating for each ConfigurableModel
        foreach (var type in configurableModelTypes)
        {
            Console.WriteLine(type.Namespace);
            var method = type.GetMethod("OnModelCreating", BindingFlags.Public | BindingFlags.Static);
            method?.Invoke(null, new object[] { builder });
        }

        base.OnModelCreating(builder);
    }
}