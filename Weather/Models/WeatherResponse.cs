namespace Weather.Models;

public class WeatherResponse
{
    public WeatherRequest Request {get; init;}

    public DateTime LocalTime {get; init;} = DateTime.Now;

    // The temperature in tenths of a degree
    public float Temperature {get; init;}

    public WeatherResponse(WeatherRequest request)
    {
        Request = request;
    }
}