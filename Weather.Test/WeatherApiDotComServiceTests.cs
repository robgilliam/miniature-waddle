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

    private static float ConvertTempCtoF(float tempC) => tempC * 5 / 9 + 32.0F;

    private void SetHttpResponse(System.Net.HttpStatusCode statusCode, string? content = null)
    {
        _protectedHttpMessageHandlerMock
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
				// TODO TESTING - Currently the mock does not care what request it gets and
				//                simply returns the specified resulte for any request.
				//
				//                It should be set up to only respond to the correct URI, including
				//                API key and expected query params, in order to ensure that that
				//                service is sending the correct request to the API.
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

    private void SetOkHttpResponse(string city, string country, DateTime timestamp, string weather, float tempC)
    {
        var timeString = timestamp.ToString("yyyy-MM-dd HH:mm");
        var tempCString = tempC.ToString("0.0");
        var tempFString = ConvertTempCtoF(tempC).ToString("0.0");

        var content = "{\n"
                    + "  \"location\": {\n"
                    +$"    \"name\": \"{city}\",\n"
                    +$"    \"country\": \"{country}\",\n"
                    +$"    \"localtime\": \"{timeString}\"\n"
                    + "  },\n"
                    + "  \"current\": {\n"
                    + "    \"condition\": {\n"
                    +$"      \"text\": \"{weather}\"\n"
                    + "    },\n"
                    +$"    \"temp_c\": {tempCString},\n"
                    +$"    \"temp_f\": {tempFString},\n"
                    + "  }\n"
                    + "}\n";

        SetHttpResponse(System.Net.HttpStatusCode.OK, content);
    }

    [TestMethod]
    public async Task GetWeatherAsync_WithCountryName_ReturnsExpectedResponse()
    {
        // Arrange
        var city = "London";
        var country = "South Africa";
        var tempType = TemperatureType.Celsius;

        var expectedTimestamp = new DateTime(2022,07,06,05,04,00); // 2022-07-06T05:04:00
        var expectedWeather = "Partly cloudy";
        var expectedTemp = 12.3F;

        SetOkHttpResponse(city, country, expectedTimestamp, expectedWeather, expectedTemp);

        var svc = new WeatherApiDotComService(_logger, _keyProvider, _httpClientFactory);
        var request = new WeatherRequest(city, country, tempType);

        // Act
        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();

        response.LocalTime.Should().Be(expectedTimestamp);
        response.Weather.Should().Be(expectedWeather);
        response.Temperature.Should().Be(expectedTemp);
        response.TemperatureType.Should().Be(tempType);
    }

    [TestMethod]
    public async Task GetWeatherAsync_WithCountryCode_ReturnsExpectedResponse()
    {
        // Arrange
        var city = "London";
        var country = "ZA";
        var tempType = TemperatureType.Celsius;

        var expectedTimestamp = new DateTime(2022,07,06,05,04,00); // 2022-07-06T05:04:00
        var expectedWeather = "Sunny";
        var expectedTemp = 29.8F;

        SetOkHttpResponse(city, country, expectedTimestamp, expectedWeather, expectedTemp);

        var svc = new WeatherApiDotComService(_logger, _keyProvider, _httpClientFactory);
        var request = new WeatherRequest(city, country, tempType);

        // Act
        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();

        response.LocalTime.Should().Be(expectedTimestamp);
        response.Weather.Should().Be(expectedWeather);
        response.Temperature.Should().Be(expectedTemp);
        response.TemperatureType.Should().Be(tempType);
    }

    [TestMethod]
    public async Task GetWeatherAsync_ForFahrenheit_ReturnsExpectedResponse()
    {
        // Arrange
        var city = "London";
        var country = "Canada";
        var tempType = TemperatureType.Fahrenheit;

        var expectedTimestamp = new DateTime(2022,07,06,05,04,00); // 2022-07-06T05:04:00
        var expectedWeather = "Sunny";
        var expectedTemp = 29.8F;

        var expectedTempF = ConvertTempCtoF(expectedTemp);

        SetOkHttpResponse(city, country, expectedTimestamp, expectedWeather, expectedTemp);

        var svc = new WeatherApiDotComService(_logger, _keyProvider, _httpClientFactory);
        var request = new WeatherRequest(city, country, tempType);

        // Act
        var response = await svc.GetWeatherAsync(request);

        // Assert
        response.Should().NotBeNull();

        response.LocalTime.Should().Be(expectedTimestamp);
        response.Weather.Should().Be(expectedWeather);

        // Can use string conversion with precision format ...
        response.Temperature.ToString("0.0").Should().Be(expectedTempF.ToString("0.0"));

        // ... or approximate floating point comparison, as preferred
        response.Temperature.Should().BeApproximately(expectedTempF, 0.1F);

        response.TemperatureType.Should().Be(tempType);
    }
}