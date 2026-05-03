
using System.Collections.Generic;

namespace Restaurant.API.Controllers
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
    }
}