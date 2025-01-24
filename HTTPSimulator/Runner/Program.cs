using HttpSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Lab12
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cities = new[] { "New York", "London", "Tokyo", "Sydney", "Berlin" };

            // initializing the HttpClient
            using var client = new HttpClient(MockHttpMessageHandlerSingleton.Instance);

            // data for all cities in parallel
            var cityWeatherTasks = cities.Select(city => FetchWeatherDataAsync(client, city));
            var processedCities = await Task.WhenAll(cityWeatherTasks);
            var validResults = processedCities.Where(result => result != null).ToList();

            if (!validResults.Any())
            {
                Console.WriteLine("No valid weather data available.");
                return;
            }

            foreach (var cityWeather in validResults)
            {
                Console.WriteLine($"City: {cityWeather.City}");
                Console.WriteLine($"Average Temperature: {cityWeather.AverageTemperature:F2}°C");
                Console.WriteLine($"Extreme Weather Days: {string.Join(", ", cityWeather.ExtremeWeatherDays)}\n");
            }

            // the city with the highest average temperature
            var cityWithHighestAvgTemp = validResults.OrderByDescending(city => city.AverageTemperature).First();
            Console.WriteLine($"City with the highest average temperature: {cityWithHighestAvgTemp.City} ({cityWithHighestAvgTemp.AverageTemperature:F2}°C)");
        }

        private static async Task<ProcessedCityWeather?> FetchWeatherDataAsync(HttpClient client, string city)
        {
            try
            {
                var apiUrl = $"https://127.0.0.1:2137/api/v13/forecast?city={city}&daily=temperature";
                var response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: Unable to fetch data for {city} (Status: {response.StatusCode})");
                    return null;
                }

                var weatherData = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();

                if (weatherData?.Daily?.Temperature == null)
                {
                    Console.WriteLine($"Error: Malformed response for {city}");
                    return null;
                }

                var dailyTemperatures = weatherData.Daily.Temperature;

                // average temperature
                var averageTemp = await CalculateAverageTemperatureAsync(dailyTemperatures);

                if (averageTemp == null)
                {
                    Console.WriteLine($"No valid temperature data for {city}.");
                    return null;
                }

                // extreme weather days
                var extremeWeatherDays = dailyTemperatures.Select((temp, index) =>
                    temp < 0 || temp > 30 ? $"Day {index + 1}" : null)
                    .Where(day => day != null)
                    .ToList();

                return new ProcessedCityWeather
                {
                    City = city,
                    AverageTemperature = averageTemp.Value,
                    ExtremeWeatherDays = extremeWeatherDays
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error while fetching data for {city}: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON response for {city}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error for {city}: {ex.Message}");
            }

            return null;
        }

        private static async Task<double?> CalculateAverageTemperatureAsync(List<double> temperatures)
        {
            return await Task.Run(() =>
            {
                if (temperatures == null || !temperatures.Any())
                {
                    Console.WriteLine("No temperatures available");
                    return (double?)null; 
                }

                return temperatures.Average();
            });
        }
    }

    public class ProcessedCityWeather
    {
        public string City { get; set; }
        public double AverageTemperature { get; set; }
        public List<string> ExtremeWeatherDays { get; set; }
    }

    public class WeatherApiResponse
    {
        public DailyWeather Daily { get; set; }
    }

    public class DailyWeather
    {
        public List<double> Temperature { get; set; }
    }
}