namespace TestTasks.WeatherFromAPI.Models
{
    using System.Collections.Generic;

    public class CityWeatherResult
    {
        public string City { get; set; }
        public string Country { get; set; }
        public long Timezone { get; set; }
        public List<WeatherMeasurementItem> Measurements { get; set; } = new List<WeatherMeasurementItem>();
    }
}