namespace TestTasks.WeatherFromAPI
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Helpers;
    using ResultType;

    public class OpenWeatherClient : IDisposable
    {
        private HttpClient _client;
        
        private OpenWeatherClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(UrlHelper.BaseUrl);
        }

        public static OpenWeatherClient GetInstance()
        {
            return new OpenWeatherClient();
        }

        public async Task<ResultContainer<TReturn>> GetAsync<TReturn>(string uri)
        {
            var response = await _client.GetAsync(uri);
            return await ProcessHttpResponse<TReturn>(response);
        }

        private static async Task<ResultContainer<TReturn>> ProcessHttpResponse<TReturn>(HttpResponseMessage response)
        {
            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                var result = await response.Content.ReadFromJsonAsync<TReturn>();

                if (result is null)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                return new ResultContainer<TReturn>
                {
                    Result = result,
                    Success = true
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResultContainer<TReturn>()
                {
                    Success = false,
                };
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}