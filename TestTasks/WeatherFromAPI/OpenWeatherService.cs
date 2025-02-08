namespace TestTasks.WeatherFromAPI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Enums;
    using Helpers;
    using Models;
    using Models.Responses;

    public class OpenWeatherService : IOpenWeatherService
    {
        public async Task<(double lat, double lon)> GetCityGeoCoordinates(string city)
        {
            var client = OpenWeatherClient.GetInstance();
            
            if (!ValidationHelper.LocationIsValid(city))
            {
                throw new ArgumentException("Invalid city");
            }

            var queryParams = new Dictionary<string, string>
            {
                { "q", city },
                { "limit", "1" },
                { "appid", "50d399710520400ca314bc92a3879f49" }
            };
            
            var uri = UrlHelper.BuildQuery(PathEnum.GeoLocation, queryParams);
            
            var response = await client.GetAsync<List<LocationResponse>>(uri);

            if (!response.Success)
            {
                throw new Exception("Unable to get city geo coordinates");
            }

            var first = response.Result.FirstOrDefault();

            if (first == null)
            {
                throw new NullReferenceException("Can't process to get city geo coordinates");
            }
            
            return (first.Latitude, first.Longitude);
        }

        public async Task<CityWeatherResult> GetWeatherByCity(double lat, double lan, int count)
        {
            var client = OpenWeatherClient.GetInstance();
            
            var queryParams = new Dictionary<string, string>
            {
                { "lat", lat.ToString(CultureInfo.InvariantCulture) },
                { "lon", lan.ToString(CultureInfo.InvariantCulture) },
                { "appid", "50d399710520400ca314bc92a3879f49" },
                { "cnt", count.ToString() },
                { "units", "metric" },
            };

            var uri = UrlHelper.BuildQuery(PathEnum.Forecast, queryParams);

            var response = await client.GetAsync<WeatherResponse>(uri);

            if (!response.Success)
            {
                throw new Exception("Unable to get city weather");
            }

            var measurements = response.Result.List
                .Select(l => new WeatherMeasurementItem
                {
                    Dt = l.Dt,
                    Temperature = l.Main?.Temp ?? 0,
                    RainVolume = l.Rain?.VolumeLast3Hours ?? 0
                }).ToList();
            
            var result = new CityWeatherResult
            {
                City = response.Result.City.Name,
                Country = response.Result.City.Country,
                Timezone = response.Result.City.Timezone,
                Measurements = measurements
            };

            return result;
        }
    }
}