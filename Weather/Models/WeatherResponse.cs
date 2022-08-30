using Newtonsoft.Json.Converters;

namespace Weather.Models;

// Serialze the temperature as the enum identifier, not its backing value
[JsonConverter(typeof(StringEnumConverter))]
public enum TemperatureType {Celsius, Fahrenheit};

public class WeatherResponse
{
    public DateTime LocalTime {get; init;} = DateTime.Now;

    public string Weather {get; init;} = "Unknown";

    public float Temperature {get; init;} = 0.0F; // Note: F is for float, here; not Fahrenheit!

    public TemperatureType TemperatureType {get; init;} = TemperatureType.Celsius;
}