using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ASPNETStarter.Server.Models;

public class SeederHistory : ConfigurableModel
{
    public int Id { get; set; }

    [StringLength(255)] public required string SeederName { get; set; }

    public DateTime LastSeededAt { get; set; }
    public int SeedPriority { get; set; }

    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<SeederHistory>();
        entity.HasIndex(i => i.SeedPriority);
        entity.HasIndex(i => i.SeederName);
    }
}