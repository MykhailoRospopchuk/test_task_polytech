namespace TestTasks.WeatherFromAPI.ResultType
{
    public class ResultContainer<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
    }
}