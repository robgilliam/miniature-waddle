using Moq;
using Moq.Protected;

using Weather.Models;
using Weather.Services;
namespace Weather.Test;

[TestClass]
public class WeatherApiDotComServiceTests
{
    private ILogger<IWeatherService> _logger;

    private IApiKeyProviderService _keyProvider;

    private IHttpClientFactory _httpClientFactory;

    private IProtectedMock<HttpMessageHandler> _protectedHttpMessageHandlerMock;

    private string API_KEY = "1234567890abcdef01234567890abcd"; // Any random string will do here

    public WeatherApiDotComServiceTests()
    {
        _logger = new NullLogger<IWeatherService>();

        // Set up mock key provider
        var keyProviderMock = new Mock<IApiKeyProviderService>();

        keyProviderMock.Setup(_ => _.GetServiceApiKey(It.Is<string>(s => s.Equals("weatherapi.com"))))
            .Returns(API_KEY);

        _keyProvider = keyProviderMock.Object;

        // Set up mock http message handled and client factory
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _protectedHttpMessageHandlerMock = httpMessageHandlerMock.Protected();

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();

        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(new HttpClient(httpMessageHandlerMock.Object));

        _httpClientFactory = httpClientFactoryMock.Object;
    }

    private void SetHttpResponse(System.Net.HttpStatusCode statusCode, string? content = null)
    {
        _protectedHttpMessageHandlerMock
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(
                new HttpResponseMessage()
                {
                    StatusCode = statusCode,
                    Content = content == null ? null : new StringContent(content)
                }
            );
    }

    private void SetOkHttpResponse(string city, string country, DateTime timestamp, string weather)
    {
        var timeString = timestamp.ToString("yyyy-MM-dd HH:mm");

        var content = "{\n"
                    + "  \"location\": {\n"
                    +$"    \"name\": \"{city}\",\n"
                    +$"    \"country\": \"{country}\",\n"
                    +$"    \"localtime\": \"{timeString}\"\n"
                    + "  },\n"
                    + "  \"current\": {\n"
                    + "    \"condition\": {\n"
                    +$"      \"text\": \"{weather}\"\n"
                    + "    }\n"
                    + "  }\n"
                    + "}\n";

        Console.Write(content);

        SetHttpResponse(System.Net.HttpStatusCode.OK, content);
    }

    [TestMethod]
    public async Task GetWeatherAsync_WithCountryName_ReturnsExpectedResponse()
    {
        // Arrange
        var city = "London";
        var country = "South Africa";
        var expectedTimestamp = new DateTime(2022,07,06,05,04,00); // 2022-07-06T05:04:00
        var expectedWeather = "Partly cloudy";

        var request = new WeatherRequest(city, country);

        SetOkHttpResponse(city, country, expectedTimestamp, expectedWeather);

        var svc = new WeatherApiDotComService(_logger, _keyProvider, _httpClientFactory);

        // Act
        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();

        response.LocalTime.Should().Be(expectedTimestamp);
        response.Weather.Should().Be(expectedWeather);
    }

    [TestMethod]
    public async Task GetWeatherAsync_WithCountryCode_ReturnsExpectedResponse()
    {
        // Arrange
        var city = "London";
        var country = "ZA";
        var expectedTimestamp = new DateTime(2022,07,06,05,04,00); // 2022-07-06T05:04:00
        var expectedWeather = "Sunny";

        var request = new WeatherRequest(city, country);

        SetOkHttpResponse(city, country, expectedTimestamp, expectedWeather);

        var svc = new WeatherApiDotComService(_logger, _keyProvider, _httpClientFactory);

        // Act
        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();

        response.LocalTime.Should().Be(expectedTimestamp);
        response.Weather.Should().Be(expectedWeather);
    }
}