namespace TestTasks.WeatherFromAPI.Models
{
    using System;

    public class WeatherMeasurementAverageItem
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double RainVolume { get; set; }
    }
}