namespace TestTasks.WeatherFromAPI.Models
{
    using System.Text.Json.Serialization;

    public class Snow
    {
        [JsonPropertyName("3h")]
        public double VolumeLast3Hours { get; set; }
    }
}