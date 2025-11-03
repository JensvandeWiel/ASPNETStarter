using Microsoft.EntityFrameworkCore;

namespace ASPNETStarter.Server.Models;

public interface ConfigurableModel
{
    static abstract void OnModelCreating(ModelBuilder modelBuilder);
}