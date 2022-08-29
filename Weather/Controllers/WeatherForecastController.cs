using Microsoft.AspNetCore.Mvc;

namespace Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly IWeatherService _service;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                     IWeatherService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet(Name = "GetWeather")]
    public async Task<WeatherResponse> GetAsync(string city, string country)
    {
        // Get the weather data for the specified city
        var request = new WeatherRequest(city, country);

        return await _service.GetWeatherAsync(request);
    }
}
