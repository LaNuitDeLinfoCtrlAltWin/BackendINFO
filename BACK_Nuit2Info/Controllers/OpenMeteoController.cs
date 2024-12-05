using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenMeteo;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace BACK_Nuit2Info.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly OpenMeteoClient _openMeteoClient;
        private readonly string _weatherApiUrl = Environment.GetEnvironmentVariable("WEATHER_API_URL");
        private readonly HttpController httpController;

        public WeatherController()
        {
            // Initialisation du client OpenMeteo
            _openMeteoClient = new OpenMeteoClient();
            httpController = new HttpController();
        }

        List<LocationData> LocationDatas = new List<LocationData>
{
    new LocationData { Name = "North Pacific", Latitude = 30, Longitude = -160 },
    new LocationData { Name = "South Pacific", Latitude = -30, Longitude = -140 },
    new LocationData { Name = "North Atlantic", Latitude = 40, Longitude = -50 },
    new LocationData { Name = "South Atlantic", Latitude = -20, Longitude = -30 },
    new LocationData { Name = "Indian Ocean", Latitude = -10, Longitude = 70 },
    new LocationData { Name = "Southern Ocean", Latitude = -60, Longitude = 0 },
    new LocationData { Name = "Arctic Ocean", Latitude = 80, Longitude = 0 },
    new LocationData { Name = "Equatorial Pacific", Latitude = 0, Longitude = -170 },
    new LocationData { Name = "Equatorial Atlantic", Latitude = 0, Longitude = -20 },
    new LocationData { Name = "Eastern Indian Ocean", Latitude = -15, Longitude = 100 },
    new LocationData { Name = "Western Indian Ocean", Latitude = -20, Longitude = 55 },
    new LocationData { Name = "Central Pacific", Latitude = 15, Longitude = -180 },
    new LocationData { Name = "Western Pacific", Latitude = 5, Longitude = 130 },
    new LocationData { Name = "Eastern Pacific", Latitude = -5, Longitude = -100 },
    new LocationData { Name = "Northern Atlantic", Latitude = 50, Longitude = -30 },
    new LocationData { Name = "Southern Atlantic", Latitude = -50, Longitude = -10 },
    new LocationData { Name = "Central Arctic", Latitude = 85, Longitude = 135 },
    new LocationData { Name = "Northern Indian Ocean", Latitude = 10, Longitude = 80 },
    new LocationData { Name = "Southwest Pacific", Latitude = -35, Longitude = 150 },
    new LocationData { Name = "Southwest Atlantic", Latitude = -45, Longitude = -60 },
        };




        /// <summary>
        /// Obtenir les données météo pour toutes les coordonnées définies dans LocationDatas.
        /// </summary>
        /// <returns>Données météo actuelles pour toutes les coordonnées</returns>
        /// 

        [HttpGet(Name = "GetWeatherData")]

        public async Task<List<OpenMeteo.WeatherForecast>?> GetWeatherData()
        {
            try
            {
                var latitudes = string.Join(",", LocationDatas.Select(loc => loc.Latitude.ToString(CultureInfo.InvariantCulture)));
                var longitudes = string.Join(",", LocationDatas.Select(loc => loc.Longitude.ToString(CultureInfo.InvariantCulture)));
                var url = $"{_weatherApiUrl}?latitude={latitudes}&longitude={longitudes}&current_weather=true&hourly=temperature_2m&format=json";

                HttpResponseMessage response = await httpController.Client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var weatherData = await JsonSerializer.DeserializeAsync<List<OpenMeteo.WeatherForecast>>(
                    await response.Content.ReadAsStreamAsync(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return weatherData;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erreur API : {e.Message}");
                return null;
            }
        }


    }
}

