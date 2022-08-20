using System.Dynamic;
using System.Net.Http.Json;

namespace Weather.Services;

public class WeatherApiDotComService : IWeatherService
{
    // TODO SECURITY : DON'T STORE THIS IN PLAIN TEXT IN CODE!
    // Idea:
    //   Create and inject key retrieval service
    //   Store key in secure a secure store, e.g. Azure Key Vault
    private const string API_KEY = "<redacted>";

    ILogger _logger;

    public WeatherApiDotComService(ILogger<IWeatherService> logger)
    {
        _logger = logger;
    }

    public async Task<WeatherResponse> GetWeatherAsync(WeatherRequest request)
    {
        WeatherResponse response;

        _logger.LogTrace("WeatherApiDotComService.GetWeatherAsync - ENTRY");

        using (var client = new HttpClient())
        {
            // TODO LOGGING - consider timing the API call
            var res = await client.GetStringAsync($"https://api.weatherapi.com/v1/current.json?key={API_KEY}&q={request.City}");

            // TODO - ERROR HANDLING Better (or some!) failure handling
            dynamic? apiResponse = JsonConvert.DeserializeObject(res);

            // Extract the values we need from the response
            var localtime = apiResponse!.location.localtime.Value;
            var temp = apiResponse.current.temp_c;

            response = new WeatherResponse(request)
            {
                LocalTime = DateTime.Parse(localtime),
                Temperature = temp
            };
        }

        _logger.LogTrace("WeatherApiDotComService.GetWeatherAsync - EXIT");

        return response;
    }
}