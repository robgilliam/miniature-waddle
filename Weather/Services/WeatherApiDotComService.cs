using System.Dynamic;
using System.Net.Http.Json;

namespace Weather.Services;

public class WeatherApiDotComService : IWeatherService
{
    private const string SERVICE_ID = "weatherapi.com";

    private readonly ILogger _logger;

    private readonly IApiKeyProviderService _keyProviderservice;

    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherApiDotComService(ILogger<IWeatherService> logger,
                                   IApiKeyProviderService keyProviderService,
                                   IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _keyProviderservice = keyProviderService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<WeatherResponse> GetWeatherAsync(WeatherRequest request)
    {
        WeatherResponse response;

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("WeatherApiDotComService.GetWeatherAsync - ENTRY");
            _logger.LogTrace($"WeatherApiDotComService.GetWeatherAsync - request.City='{request.City}'");
            _logger.LogTrace($"WeatherApiDotComService.GetWeatherAsync - request.Countr='{request.Country}'");
            _logger.LogTrace($"WeatherApiDotComService.GetWeatherAsync - request.TemperatureType='{request.TemperatureType.ToString()}'");
        }

        var apiKey = _keyProviderservice.GetServiceApiKey(SERVICE_ID);

        // TODO ERROR HANDLING - Handle if apiKey is null (unknown service/no keyfile)

        var client = _httpClientFactory.CreateClient();

        // TODO LOGGING - consider timing the API call
        var res = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={request.City},{request.Country}");

        // TODO ERROR HANDLING - Better (or some!) failure handling
        dynamic? apiResponse = JsonConvert.DeserializeObject(res);

        // Extract the values we need from the API response
        var localtime = apiResponse!.location.localtime.Value;
        var weather = apiResponse.current.condition.text;

		// Select the temperature to be returned based on the requested temperature type
        var temperature = request.TemperatureType == TemperatureType.Celsius
		                  ? apiResponse.current.temp_c
						  : apiResponse.current.temp_f;

        response = new WeatherResponse()
        {
            LocalTime = DateTime.Parse(localtime),
            Weather = weather,
            Temperature = temperature,
            TemperatureType = request.TemperatureType
        };

        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace("WeatherApiDotComService.GetWeatherAsync - EXIT");
            _logger.LogTrace($"WeatherApiDotComService.GetWeatherAsync - response.LocalTime='{response.LocalTime}'");
            _logger.LogTrace($"WeatherApiDotComService.GetWeatherAsync - response.Weather='{response.Weather}'");
            _logger.LogTrace($"WeatherApiDotComService.GetWeatherAsync - response.Temperature='{response.Temperature.ToString("0.0")}'");
            _logger.LogTrace($"WeatherApiDotComService.GetWeatherAsync - response.TemperatureType='{response.TemperatureType}'");
        }

        return response;
    }
}