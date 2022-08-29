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
                Weather = "Sunny"
            };
        });

        _logger.LogTrace("DummyWeatherService.GetWeatherAsync - EXIT");

        return response;
    }
}