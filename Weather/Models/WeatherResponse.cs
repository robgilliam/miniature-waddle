namespace Weather.Models;

public class WeatherResponse
{
    public WeatherRequest Request {get; init;}

    public DateTime LocalTime {get; init;} = DateTime.Now;

    public string Weather {get; init;} = "Unknown";

    public WeatherResponse(WeatherRequest request)
    {
        Request = request;
    }
}