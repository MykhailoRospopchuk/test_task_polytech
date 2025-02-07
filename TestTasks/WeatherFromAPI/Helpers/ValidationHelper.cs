namespace TestTasks.WeatherFromAPI.Helpers
{
    using System.Text.RegularExpressions;

    public class ValidationHelper
    {
        public static bool LocationIsValid(string input)
        {
            string pattern = @"^[A-Za-z]+(?:,[A-Za-z]{2,3})?$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool DayRangeIsValid(int input)
        {
            return input is > 0 and < 5;
        }
    }
}