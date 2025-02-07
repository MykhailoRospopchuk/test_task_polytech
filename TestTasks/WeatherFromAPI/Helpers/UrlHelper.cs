namespace TestTasks.WeatherFromAPI.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Encodings.Web;
    using Enums;

    public class UrlHelper
    {
        public static readonly string BaseUrl = "http://api.openweathermap.org/";

        private static readonly IDictionary<PathEnum, string> Path = new Dictionary<PathEnum, string>()
        {
            { PathEnum.Forecast, "data/2.5/forecast" },
            { PathEnum.GeoLocation, "geo/1.0/direct" },
        };

        public static string BuildQuery(PathEnum path, IEnumerable<KeyValuePair<string, string>> queryParams)
        {
            if (!Path.TryGetValue(path, out string uri))
            {
                throw new ArgumentException("Invalid uri");
            }

            var sb = new StringBuilder();
            sb.Append(BaseUrl);
            sb.Append(uri);

            bool first = true;
            
            foreach (var parameter in queryParams)
            {
                if (string.IsNullOrEmpty(parameter.Value))
                {
                    continue;
                }

                sb.Append(first ? '?': '&');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                first = false;
            }

            return sb.ToString();
        }
    }
}