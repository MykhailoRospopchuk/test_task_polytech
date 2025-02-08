using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTasks.WeatherFromAPI.Models;

namespace TestTasks.WeatherFromAPI
{
    using System.Linq;
    using Helpers;

    public class WeatherManager
    {
        private readonly IOpenWeatherService _apiClient;

        public WeatherManager(IOpenWeatherService service)
        {
            _apiClient = service;
        }

        public WeatherManager()
        {
            _apiClient = new OpenWeatherService();
        }

        public async Task<WeatherComparisonResult> CompareWeather(string cityA, string cityB, int dayCount)
        {
            if (!ValidationHelper.DayRangeIsValid(dayCount))
            {
                throw new ArgumentException("Invalid day count");
            }
            
            var count = DateHelper.GetCountFromDayNumber(dayCount);
            
            var ciryAGeoCoordinates = await _apiClient.GetCityGeoCoordinates(cityA);
            var ciryBGeoCoordinates = await _apiClient.GetCityGeoCoordinates(cityB);
            
            var cityAWeatherData = await _apiClient.GetWeatherByCity(ciryAGeoCoordinates.lat, ciryAGeoCoordinates.lon, count);
            var cityBWeatherData = await _apiClient.GetWeatherByCity(ciryBGeoCoordinates.lat, ciryBGeoCoordinates.lon, count);

            var cityAMeasurements = PrepareWeatherMeasurements(cityAWeatherData);
            var cityBMeasurements = PrepareWeatherMeasurements(cityBWeatherData);

            var cityAMeasurementAverage = PrepareWeatherMeasurementsAverage(cityAMeasurements, dayCount);
            var cityBMeasurementAverage = PrepareWeatherMeasurementsAverage(cityBMeasurements, dayCount);

            var counting = CompareWeather(cityAMeasurementAverage, cityBMeasurementAverage);

            var result = new WeatherComparisonResult(
                $"{cityAWeatherData.City} {cityAWeatherData.Country}",
                $"{cityBWeatherData.City} {cityBWeatherData.Country}", 
                counting.warmerCount,
                counting.rainerCount);
            
            return result;
        }

        private (int warmerCount, int rainerCount) CompareWeather(
            Dictionary<DateTime, (double temperatureAvg, double rainAvg)> cityA,
            Dictionary<DateTime, (double temperatureAvg, double rainAvg)> cityB)
        {
            var warmerCount = 0;
            var rainerCount = 0;

            foreach (var dic in cityA)
            {
                if (cityB.TryGetValue(dic.Key, out var dicValue))
                {
                    if (dic.Value.temperatureAvg >  dicValue.temperatureAvg)
                    {
                        warmerCount++;
                    }
                    
                    if (dic.Value.rainAvg >  dicValue.rainAvg)
                    {
                        rainerCount++;
                    }
                }
            }

            return (warmerCount, rainerCount);
        }

        private Dictionary<DateTime,(double temperatureAvg, double rainAvg)> PrepareWeatherMeasurementsAverage(
            List<WeatherMeasurementItem> measurements,
            int dayCount)
        {
            var result2 = measurements
                .GroupBy(m => DateHelper.UnixToDateTime(m.Dt).Date)
                .Take(dayCount)
                .ToDictionary(
                    g => g.Key, 
                    g => 
                        (g.Average(m => m.Temperature), 
                        g.Average(m => m.RainVolume))
                    );
            
            return result2;
        }

        private List<WeatherMeasurementItem> PrepareWeatherMeasurements(CityWeatherResult cityWeatherResult)
        {
            var measurements = cityWeatherResult.Measurements;

            foreach (var item in measurements)
            {
                item.Dt += cityWeatherResult.Timezone;
            }

            return measurements;
        }
    }
}
