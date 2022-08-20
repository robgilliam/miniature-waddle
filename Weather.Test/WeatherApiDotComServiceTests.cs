using Weather.Models;
using Weather.Services;
namespace Weather.Test;

[TestClass]
public class WeatherApiDotComServiceTests
{
    private ILogger<IWeatherService> _logger;

    public WeatherApiDotComServiceTests()
    {
        _logger = new NullLogger<IWeatherService>();
    }


    [TestMethod]
    public async Task GetWeatherAsync_Always_ReturnsResponse()
    {
        // Arrange
        var request = new WeatherRequest("London");

        // Act
        var svc = new WeatherApiDotComService(_logger);

        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();
    }
}