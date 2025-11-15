using System.Net.Http.Headers;
using System.Net.Http.Json;
using ASPNETStarter.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Helpers;

public class AuthHelper
{
    private readonly WebAppFactory _factory;

    public AuthHelper(WebAppFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    ///     Creates a new test user with optional roles in the database
    /// </summary>
    public async Task<ApplicationUser> CreateTestUserAsync(
        string? email = "testuser@test.com",
        string password = "Test123!",
        params string[] roles)
    {
        // Generate a unique email to ensure fresh user creation
        email = $"{Guid.NewGuid()}_{email}";
        TestLogger.Logger.Information($"Creating test user with email: {email}");

        ApplicationUser user;
        using (var scope = _factory.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            TestLogger.Logger.Information("Creating user in database: {Email}", email);
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errorMessage =
                    $"Failed to create test user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                TestLogger.Logger.Error(errorMessage);
                throw new Exception(errorMessage);
            }

            TestLogger.Logger.Information("User created successfully: {Email} (ID: {UserId}) with roles: {Roles}", email, user.Id, roles);

            // Assign roles if provided
            if (roles.Length > 0)
                foreach (var role in roles)
                {
                    TestLogger.Logger.Information("Processing role: {Role}", role);

                    // Ensure role exists
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        TestLogger.Logger.Information("Creating role: {Role}", role);
                        var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                        if (!roleResult.Succeeded)
                        {
                            var errorMessage =
                                $"Failed to create role {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}";
                            TestLogger.Logger.Error(errorMessage);
                            throw new Exception(errorMessage);
                        }
                    }

                    // Add user to role
                    TestLogger.Logger.Information("Adding user {UserId} to role: {Role}", user.Id, role);
                    var addRoleResult = await userManager.AddToRoleAsync(user, role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errorMessage =
                            $"Failed to add user to role {role}: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}";
                        TestLogger.Logger.Error(errorMessage);
                        throw new Exception(errorMessage);
                    }
                }
        }

        TestLogger.Logger.Information("Test user created successfully: {Email}", email);
        return user;
    }

    /// <summary>
    ///     Returns an authenticated HttpClient with a valid bearer token for the given user
    /// </summary>
    public async Task<HttpClient> GetAuthenticatedClientAsync(
        ApplicationUser user,
        string password = "Test123!")
    {
        TestLogger.Logger.Information("Authenticating user: {Email}", user.Email!);

        // Login and get token
        var client = _factory.CreateClient();
        var loginPayload = new { email = user.Email, password };

        TestLogger.Logger.Information("HTTP Request: POST /auth/login");

        var loginResponse = await client.PostAsJsonAsync("/auth/login", loginPayload);

        TestLogger.Logger.Information("HTTP Response: {StatusCode} {ReasonPhrase}", (int)loginResponse.StatusCode, loginResponse.ReasonPhrase);

        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        if (string.IsNullOrEmpty(loginResult?.AccessToken))
        {
            TestLogger.Logger.Error("Failed to get access token from login response");
            throw new Exception("Failed to get access token from login response");
        }

        TestLogger.Logger.Information("Successfully obtained access token");

        // Create new client with auth header
        var authenticatedClient = _factory.CreateClient();
        authenticatedClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResult.AccessToken);

        return authenticatedClient;
    }

    private class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}