namespace TestTasks.WeatherFromAPI.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class WeatherData
    {
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("main")]
        public MainData Main { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherCondition> Weather { get; set; } =  new List<WeatherCondition>();

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; }

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("pop")]
        public double Pop { get; set; }

        [JsonPropertyName("rain")]
        public Rain Rain { get; set; }

        [JsonPropertyName("snow")]
        public Snow Snow { get; set; }

        [JsonPropertyName("sys")]
        public Sys Sys { get; set; }

        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; }
    }
}