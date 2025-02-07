namespace TestTasks.WeatherFromAPI.Helpers
{
    using System;

    public class DateHelper
    {
        public static int GetCountFromDayNumber(int dayNumber)
        {
            return dayNumber * 8;
        }
        
        public static DateTime UnixToDateTime(long unixTimeStamp)
        {
            // Convert Unix timestamp to DateTime (UTC)
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
        }
    }
}