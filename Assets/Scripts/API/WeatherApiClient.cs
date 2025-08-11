using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Modern API client for fetching weather data
    /// Students will complete the implementation following async/await patterns
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
        [Header("API Configuration")]
        [SerializeField] private string baseUrl = "http://api.openweathermap.org/data/2.5/weather";
        
        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// TODO: Students will implement this method
        /// </summary>
        /// <param name="city">City name to get weather for</param>
        /// <returns>WeatherData object or null if failed</returns>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }
            
            // Check if API key is configured
            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in StreamingAssets folder.");
                return null;
            }

            // TODO: Build the complete URL with city and API key
            string apiKey = ApiConfig.OpenWeatherMapApiKey;
            string url = $"{baseUrl}?q={city}&appid={apiKey}&units=metric";

            // TODO: Create UnityWebRequest and use modern async pattern
            using UnityWebRequest request = UnityWebRequest.Get(url);
            // TODO: Use async/await, send the request and wait for response
            var operation = request.SendWebRequest(); // start of request 
            while (!operation.isDone) await Task.Yield(); // wating for request while not blocking.
                                                          // TODO: Implement proper error handling for different result types
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError ||
                request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError($"Request Error: {request.error}");
                return null;
            }
            // Check request.result for Success, ConnectionError, ProtocolError, DataProcessingError
            try
            {
                string json = request.downloadHandler.text;
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                return weatherData;
            }
            // TODO: Parse JSON response using Newtonsoft.Json

            // TODO: Return the parsed WeatherData object
            catch (Exception ex)
            {
                Debug.LogError($"Failed to parse weather data: {ex.Message}");
                return null;
            }

            return null; // Placeholder - students will replace this
        }
        
        /// <summary>
        /// Example usage method - students can use this as reference
        /// </summary>
        private async void Start()
        {
            // Example: Get weather for London
            var weatherData = await GetWeatherDataAsync("London");
            
            if (weatherData != null && weatherData.IsValid)
            {
                Debug.Log($"Weather in {weatherData.CityName}: {weatherData.TemperatureInCelsius:F1}°C");
                Debug.Log($"Description: {weatherData.PrimaryDescription}");
            }
            else
            {
                Debug.LogError("Failed to get weather data");
            }
        }
    }
}