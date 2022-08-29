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

        _logger.LogTrace("WeatherApiDotComService.GetWeatherAsync - ENTRY");

        var apiKey = _keyProviderservice.GetServiceApiKey(SERVICE_ID);

        // TODO ERROR HANDLING - Handle if apiKey is null (unknown service)

        var client = _httpClientFactory.CreateClient();

        // TODO LOGGING - consider timing the API call
        var res = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={request.City}");

        // TODO ERROR HANDLING - Better (or some!) failure handling
        dynamic? apiResponse = JsonConvert.DeserializeObject(res);

        // Extract the values we need from the API response
        var localtime = apiResponse!.location.localtime.Value;
        var temp = apiResponse.current.temp_c;

        response = new WeatherResponse(request)
        {
            LocalTime = DateTime.Parse(localtime),
            Temperature = temp
        };

        _logger.LogTrace("WeatherApiDotComService.GetWeatherAsync - EXIT");

        return response;
    }
}