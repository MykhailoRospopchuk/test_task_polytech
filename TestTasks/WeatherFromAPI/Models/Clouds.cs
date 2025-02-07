namespace TestTasks.WeatherFromAPI.Models
{
    using System.Text.Json.Serialization;

    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}