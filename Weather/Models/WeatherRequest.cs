namespace Weather.Models;

public class WeatherRequest
{
    public string City {get;}
    public string? Country {get;}

    public WeatherRequest( string city, string? country = null)
    {
        City = city;
        Country = country;
    }
}