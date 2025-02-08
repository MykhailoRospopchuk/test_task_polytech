namespace XUnitTest;

using Moq;
using TestTasks.WeatherFromAPI;
using TestTasks.WeatherFromAPI.Models;

public class WeatherManagerTests
{
    private readonly Mock<IOpenWeatherService> _mockApiClient;
    private readonly WeatherManager _weatherManager;

    public WeatherManagerTests()
    {
        _mockApiClient = new Mock<IOpenWeatherService>();
        _weatherManager = new WeatherManager(_mockApiClient.Object);
    }

    [Fact]
    public async Task CompareWeather_ShouldReturnCorrectResult()
    {
        // Arrange
        var cityA = "odesa,ua";
        var cityB = "lviv,ua";
        var dayCount = 4;

        var cityAGeoCoordinates = (lat: 46.4843, lon: 30.7323);
        var cityBGeoCoordinates = (lat: 49.8419, lon: 24.0316);

        var cityAWeatherData = new CityWeatherResult
        {
            City = "Odesa",
            Country = "UA",
            Timezone = 7200,
            Measurements = new List<WeatherMeasurementItem>
            {
                new WeatherMeasurementItem { Dt = 1633036800, Temperature = 20, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633047600, Temperature = 21, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633058400, Temperature = 15, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633069200, Temperature = 12, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633080000, Temperature = 12, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633090800, Temperature = 17, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633101600, Temperature = 29, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633112400, Temperature = 24, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633123200, Temperature = 21, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633134000, Temperature = 17.5, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633144800, Temperature = 12.5, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633155600, Temperature = 14, RainVolume = 0 },
            }
        };

        var cityBWeatherData = new CityWeatherResult
        {
            City = "Lviv",
            Country = "UA",
            Timezone = 7200,
            Measurements = new List<WeatherMeasurementItem>
            {
                new WeatherMeasurementItem { Dt = 1633036800, Temperature = 5, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633047600, Temperature = 2, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633058400, Temperature = 5, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633069200, Temperature = 6, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633080000, Temperature = 1, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633090800, Temperature = 7, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633101600, Temperature = 2, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633112400, Temperature = 23, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633123200, Temperature = 1, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633134000, Temperature = 22.5, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633144800, Temperature = 12, RainVolume = 0 },
                new WeatherMeasurementItem { Dt = 1633155600, Temperature = 14, RainVolume = 0 },
            }
        };

        _mockApiClient.Setup(x => x.GetCityGeoCoordinates(cityA)).ReturnsAsync(cityAGeoCoordinates);
        _mockApiClient.Setup(x => x.GetCityGeoCoordinates(cityB)).ReturnsAsync(cityBGeoCoordinates);
        _mockApiClient.Setup(x => x.GetWeatherByCity(cityAGeoCoordinates.lat, cityAGeoCoordinates.lon, It.IsAny<int>())).ReturnsAsync(cityAWeatherData);
        _mockApiClient.Setup(x => x.GetWeatherByCity(cityBGeoCoordinates.lat, cityBGeoCoordinates.lon, It.IsAny<int>())).ReturnsAsync(cityBWeatherData);

        // Act
        var result = await _weatherManager.CompareWeather(cityA, cityB, dayCount);

        // Assert
        Assert.Equal("Odesa UA", result.CityA);
        Assert.Equal("Lviv UA", result.CityB);
        Assert.Equal(2, result.WarmerDaysCount);
        Assert.Equal(0, result.RainierDaysCount);
    }

    [Fact]
    public async Task CompareWeather_InvalidDayCount_ShouldThrowArgumentException()
    {
        // Arrange
        var cityA = "odesa,ua";
        var cityB = "lviv,ua";
        var dayCount = 6; // Invalid day count

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _weatherManager.CompareWeather(cityA, cityB, dayCount));
    }
}