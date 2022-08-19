namespace Weather.Services;

public interface IWeatherService
{
    Task<WeatherResponse> GetWeatherAsync(WeatherRequest request);
}