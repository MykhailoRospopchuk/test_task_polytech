namespace TestTasks.WeatherFromAPI.Models
{
    using System.Text.Json.Serialization;

    public class Coord
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }
    }
}