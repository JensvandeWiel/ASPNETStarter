using ASPNETStarter.Server;
using ASPNETStarter.Server.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.XUnit3;
using Testcontainers.MsSql;

namespace Tests;

public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer = new MsSqlBuilder()
        .WithPassword("YourStrong!Passw0rd")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _sqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _sqlContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(context =>
        {
            var descriptor = context.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null) context.Remove(descriptor);

            context.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_sqlContainer.GetConnectionString() + ";Database=testing");
            });

            // Configure Serilog to log to xUnit output
            context.AddLogging(x => x.ClearProviders().AddSerilog(new LoggerConfiguration().WriteTo
                .XUnit3TestOutput()
                .CreateLogger()));
        });
    }
}