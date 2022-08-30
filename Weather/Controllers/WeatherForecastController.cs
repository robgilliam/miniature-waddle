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
    public async Task<WeatherResponse> GetAsync(string city, string country, string temperatureType)
    {
		// TODO VALIDATION - Validate temperature type properly
		//
		//                   Suggest: create and inject a parameter validation service
		//                   which can be unit-tested separately
		//
		//                   Or: pass received query parameters off to a WeatherRequest
		//                   builder service that validates the parameters and returns
		//                   the  appropriate WeatherRequest object or an error/throws an
		//                   exception if the parameters can't be used.
		var tempType = temperatureType.StartsWith("F") ? TemperatureType.Fahrenheit : TemperatureType.Celsius;

        // Get the weather data for the specified parameters
        var request = new WeatherRequest(city, country, tempType);

        return await _service.GetWeatherAsync(request);
    }
}
