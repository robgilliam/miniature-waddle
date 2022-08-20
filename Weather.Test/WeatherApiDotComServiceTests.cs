using Moq;

using Weather.Models;
using Weather.Services;
namespace Weather.Test;

[TestClass]
public class WeatherApiDotComServiceTests
{
    private ILogger<IWeatherService> _logger;

    private string API_KEY = "<redacted>";

    public WeatherApiDotComServiceTests()
    {
        _logger = new NullLogger<IWeatherService>();
    }


    [TestMethod]
    public async Task GetWeatherAsync_Always_ReturnsResponse()
    {
        // Arrange
        var request = new WeatherRequest("London");
        var keyProvider = new Mock<IApiKeyProviderService>();
        keyProvider.Setup(kp => kp.GetServiceApiKey(It.Is<string>(s => s.Equals("weatherapi.com"))))
                   .Returns(API_KEY);

        var svc = new WeatherApiDotComService(_logger, keyProvider.Object);

        // Act
        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();
    }
}