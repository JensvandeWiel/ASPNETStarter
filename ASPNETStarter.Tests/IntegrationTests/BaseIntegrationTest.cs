using ASPNETStarter.Server.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Helpers;

namespace Tests.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<WebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly AuthHelper AuthHelper;
    protected readonly HttpClient Client;
    protected readonly WebAppFactory Factory;

    public BaseIntegrationTest(WebAppFactory factory)
    {
        TestLogger.Initialize();

        Factory = factory;
        Client = factory.CreateClient();
        AuthHelper = new AuthHelper(factory);
        _scope = factory.Services.CreateScope();

        var dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        TestLogger.Logger.Information("Deleting database: ApplicationDbContext");
        dbContext.Database.EnsureDeleted();

        TestLogger.Logger.Information("Migrating database: ApplicationDbContext");
        dbContext.Database.Migrate();

        TestLogger.Logger.Information("Test environment initialized successfully");
    }
}