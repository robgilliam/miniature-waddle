namespace Weather.Models;

public class WeatherRequest
{
    public string City {get;}
    public string Country {get;}
    public TemperatureType TemperatureType {get;}

    public WeatherRequest( string city, string country, TemperatureType temperatureType)
    {
        City = city;
        Country = country;
        TemperatureType = temperatureType;
    }
}