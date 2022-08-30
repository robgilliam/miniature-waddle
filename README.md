
# miniature-waddle

Experian tech test

* Requires .NET 6.0

## To build

* From the _root_ directory (i.e. the directory containing `Weather.sln`), run `dotnet build`

## To run the unit tests

* From the _root_ directory run `dotnet test`

## To run the service

* From the _root_ directory run `dotnet run --project Weather`

*OR*

* Change to the `Weather` directory and run `dotnet run`

The port used to access the API is logged on startup, and can be changed by editing the `applicationUrl` value in the `launchSettings.json` file

### Specifying the API key

In order for the service to access the weatherapi.com API, you will need to specify your API key, obtained from weatherapi.com. To do this add a key file called `weatherapi.key` to the `Weather` folder, and add the following line:

    weatherapi.com:{key}
    
where `{key}` is your API key, e.g.:

    weatherapi.com:f087593ea74f438293b161594221808

(note: the above example is not a valid key, or shouldn't be!)

## To access the service

* Send HTTP/1.1 GET requests to the following URL:

    https://localhost:{port}/WeatherForecast?city={city}&country={country}&TemperatureType={tempType}
    
   where `{port}` is the port noted above, `{city}` and `{country}` specify the location to retrieve the weather for, and `{tempType}` is `Celsius`, `Centigrade`, or `Fahrenheit`.
   
   Note that {country} can be the full name or the ISO 3166-1 alpha-2 code for the country (e.g. `Germany` or `DE`)
 
 *OR*
 
 * Navigate to
 
     https://localhost:{port}/swagger/index.html

   to use Swagger to inspect the interface and send sample requests.

## Logging

Logging is performed via the built-in logging included in ASP.NET Core. To change the logging level, edit the settings in `appsettings.json` or `appsettings.Development.json`, as appropriate.
