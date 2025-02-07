namespace TestTasks.WeatherFromAPI.Models
{
    using System.Text.Json.Serialization;

    public class Sys
    {
        [JsonPropertyName("pod")]
        public string Pod { get; set; }
    }
}