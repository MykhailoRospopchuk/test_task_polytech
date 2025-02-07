namespace TestTasks.WeatherFromAPI.Models
{
    using System.Text.Json.Serialization;

    public class Rain
    {
        [JsonPropertyName("3h")]
        public double VolumeLast3Hours { get; set; }
    }
}