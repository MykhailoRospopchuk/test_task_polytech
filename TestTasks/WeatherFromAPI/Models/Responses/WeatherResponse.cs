namespace TestTasks.WeatherFromAPI.Models.Responses
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class WeatherResponse
    {
        [JsonPropertyName("cod")]
        public string Cod { get; set; }

        [JsonPropertyName("message")]
        public int Message { get; set; }

        [JsonPropertyName("cnt")]
        public int Cnt { get; set; }

        [JsonPropertyName("list")]
        public List<WeatherData> List { get; set; } = new List<WeatherData>();

        [JsonPropertyName("city")]
        public City City { get; set; }
    }
}