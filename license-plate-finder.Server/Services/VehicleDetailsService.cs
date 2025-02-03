using license_plate_finder.Server.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using DotNetEnv;

namespace license_plate_finder.Server.Services
{
    public class VehicleDetailsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<VehicleDetailsService> _logger;
        private string _rdwApiUrl;
        private string _apiKey;
        private const string _apiQuery = "?$where=kenteken='{0}'";

        public VehicleDetailsService(HttpClient httpClient, IMemoryCache cache, ILogger<VehicleDetailsService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;

            Env.Load();
            _rdwApiUrl = Env.GetString("RDW_API_URL");
            _apiKey = Env.GetString("API_KEY");
        }
        public async Task<VehicleDetails?> GetVehicleDetailsAsync(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                return null;

            licensePlate = FormatLicensePlate(licensePlate);

            if (_cache.TryGetValue(licensePlate, out VehicleDetails cachedVehicle))
            {
                _logger.LogInformation($"Cache hit for {licensePlate}");
                return cachedVehicle;
            }

            try
            {
                var apiUrlWithToken = $"{_rdwApiUrl}{string.Format(_apiQuery, licensePlate)}&$$app_token={_apiKey}";

                var response = await _httpClient.GetAsync(apiUrlWithToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"API returned: {response.StatusCode} for {licensePlate}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var vehicles = JsonSerializer.Deserialize<List<VehicleDetails>>(json);

                if (vehicles == null || vehicles.Count == 0)
                {
                    _logger.LogWarning($"No vehicle details found for {licensePlate}");
                    return null;
                }

                var vehicle = vehicles.FirstOrDefault();

                // Store in cache for 10 minutes
                _cache.Set(licensePlate, vehicle, TimeSpan.FromMinutes(10));

                _logger.LogInformation($"Fetched vehicle details for {licensePlate}");
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching vehicle details for {licensePlate}");
                return null;
            }
        }

        private string FormatLicensePlate(string licensePlate)
        {
            return licensePlate.Replace("-", "").ToUpper();
        }
    }
}