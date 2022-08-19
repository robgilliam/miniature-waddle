namespace Weather.Services;

public class DummyWeatherService : IWeatherService
{
    ILogger _logger;

    public DummyWeatherService(ILogger<IWeatherService> logger)
    {
        _logger = logger;
    }

    public async Task<WeatherResponse> GetWeatherAsync(WeatherRequest request)
    {
        _logger.LogTrace("DummyWeatherService.GetWeatherAsync - ENTRY");

        var response = await Task.Run(() => {
            return new WeatherResponse(request)
            {
                LocalTime = DateTime.Now,
                Temperature = 12.3F // Note: that's F for float, not Fahrenheit!
            };
        });

        _logger.LogTrace("DummyWeatherService.GetWeatherAsync - EXIT");

        return response;
    }
}