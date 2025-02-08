namespace TestTasks.WeatherFromAPI
{
    using System.Threading.Tasks;
    using Models;

    public interface IOpenWeatherService
    {
        Task<(double lat, double lon)> GetCityGeoCoordinates(string city);
        Task<CityWeatherResult> GetWeatherByCity(double lat, double lan, int count);
    }
}