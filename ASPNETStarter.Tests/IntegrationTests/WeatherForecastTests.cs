using Tests.Helpers;

namespace Tests.IntegrationTests;

public class WeatherForecastTests : BaseIntegrationTest
{
    public WeatherForecastTests(WebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetWeatherForecasts_ShouldReturnOkResponse()
    {
        // Arrange
        TestLogger.Logger.Information("Test: {TestName}", nameof(GetWeatherForecasts_ShouldReturnOkResponse));
        TestLogger.Logger.Information("Verify that authenticated user can retrieve weather forecasts");

        try
        {
            var user = await AuthHelper.CreateTestUserAsync();
            TestLogger.Logger.Information("User created with ID: {UserId}, Email: {Email}", user.Id, user.Email);

            var authenticatedClient = await AuthHelper.GetAuthenticatedClientAsync(user);
            TestLogger.Logger.Information("Authenticated client created successfully");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/WeatherForecast");
            TestLogger.Logger.Information("HTTP Request: GET /api/v1/WeatherForecast");

            // Act
            var response = await authenticatedClient.SendAsync(request);
            TestLogger.Logger.Information("HTTP Response: {StatusCode} {ReasonPhrase}", (int)response.StatusCode,
                response.ReasonPhrase);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            TestLogger.Logger.Information("Response validation: IsEmpty={IsEmpty}, ContentLength={ContentLength}",
                string.IsNullOrEmpty(content), content.Length);

            Assert.False(string.IsNullOrEmpty(content));
            TestLogger.Logger.Information("Test completed: {TestName}",
                nameof(GetWeatherForecasts_ShouldReturnOkResponse));
        }
        catch (Exception ex)
        {
            TestLogger.Logger.Error(ex, "Test failed: {TestName}, Error: {Error}",
                nameof(GetWeatherForecasts_ShouldReturnOkResponse), ex.Message);
            throw;
        }
    }
}