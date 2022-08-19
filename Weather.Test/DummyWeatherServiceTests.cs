using Weather.Models;
using Weather.Services;
namespace Weather.Test;

[TestClass]
public class DummyWeatherServiceTests
{
    private ILogger<IWeatherService> _logger;

    public DummyWeatherServiceTests()
    {
        _logger = new NullLogger<IWeatherService>();
    }


    [TestMethod]
    public async Task GetWeatherAsync_Always_ReturnsResponse()
    {
        // Arrange
        var request = new WeatherRequest("London");

        var svc = new DummyWeatherService(_logger);

        // Act
        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();
    }
}