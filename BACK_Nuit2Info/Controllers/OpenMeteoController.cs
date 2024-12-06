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
    new LocationData { Name = "South Atlantic", Latitude = -30, Longitude = -40 },
    new LocationData { Name = "North Atlantic", Latitude = 40, Longitude = -60 },
    new LocationData { Name = "Central Atlantic", Latitude = 20, Longitude = -50 },
    new LocationData { Name = "South Indian Ocean", Latitude = -20, Longitude = 55 },
    new LocationData { Name = "Eastern Indian Ocean", Latitude = -40, Longitude = 80 },
    new LocationData { Name = "Central Indian Ocean", Latitude = 10, Longitude = 90 },
    new LocationData { Name = "South Pacific Ocean", Latitude = -45, Longitude = -170 },
    new LocationData { Name = "Equatorial Pacific Ocean", Latitude = 0, Longitude = -140 },
    new LocationData { Name = "Central Pacific Ocean", Latitude = 20, Longitude = -120 },
    new LocationData { Name = "Gulf of Alaska", Latitude = 60, Longitude = -150 },
    new LocationData { Name = "Southeast Atlantic", Latitude = -20, Longitude = -30 },
    new LocationData { Name = "Drake Passage", Latitude = -60, Longitude = -50 },
    new LocationData { Name = "Southeast Atlantic", Latitude = -15, Longitude = -5 },
    new LocationData { Name = "Eastern Mediterranean", Latitude = 10, Longitude = 30 },
    new LocationData { Name = "Strait of Gibraltar", Latitude = 36, Longitude = -5 },
    new LocationData { Name = "North Sea", Latitude = 60, Longitude = 20 },
    new LocationData { Name = "Barents Sea", Latitude = 70, Longitude = 30 },
    new LocationData { Name = "English Channel", Latitude = 50, Longitude = -5 },
    new LocationData { Name = "Baltic Sea", Latitude = 50, Longitude = 15 },
    new LocationData { Name = "White Sea", Latitude = 60, Longitude = 40 },
    new LocationData { Name = "Eastern Pacific", Latitude = -15, Longitude = -100 },
    new LocationData { Name = "Southeast Pacific", Latitude = -20, Longitude = -130 },
    new LocationData { Name = "Central South Pacific", Latitude = -5, Longitude = -170 },
    new LocationData { Name = "Western Pacific", Latitude = 0, Longitude = 140 },
    new LocationData { Name = "Southwest Pacific", Latitude = -40, Longitude = 160 },
    new LocationData { Name = "Solomon Sea", Latitude = 10, Longitude = 160 },
    new LocationData { Name = "Coral Sea", Latitude = -10, Longitude = 150 },
    new LocationData { Name = "Western Pacific Central", Latitude = 20, Longitude = 140 },
    new LocationData { Name = "Sea of Okhotsk", Latitude = 50, Longitude = 145 },
    new LocationData { Name = "Bering Sea", Latitude = 60, Longitude = 170 },
    new LocationData { Name = "North Pacific", Latitude = 30, Longitude = 180 },
    new LocationData { Name = "Southern Ocean", Latitude = -60, Longitude = 0 },
    new LocationData { Name = "Southeast Southern Ocean", Latitude = -70, Longitude = -80 },
    new LocationData { Name = "Southern Indian Ocean", Latitude = -50, Longitude = 100 },
    new LocationData { Name = "Philippine Sea", Latitude = 15, Longitude = 120 },
    new LocationData { Name = "Arabian Sea", Latitude = 25, Longitude = 70 },
    new LocationData { Name = "Bay of Bengal", Latitude = 10, Longitude = 80 },
    new LocationData { Name = "Caspian Sea", Latitude = 45, Longitude = 50 },
    new LocationData { Name = "Southern Mediterranean", Latitude = -35, Longitude = 25 },
    new LocationData { Name = "Caribbean Sea", Latitude = 5, Longitude = -55 },
    new LocationData { Name = "Gulf of Mexico", Latitude = 18, Longitude = -75 },
    new LocationData { Name = "Northern Gulf of Mexico", Latitude = 30, Longitude = -90 },
    new LocationData { Name = "Southwest Atlantic", Latitude = -25, Longitude = -15 },
    new LocationData { Name = "Northeast Atlantic", Latitude = 40, Longitude = -20 },
    new LocationData { Name = "Southwest Atlantic", Latitude = -35, Longitude = -55 },
    new LocationData { Name = "Patagonian Atlantic", Latitude = -50, Longitude = -65 },
    new LocationData { Name = "Greenland Sea", Latitude = 60, Longitude = 10 },
    new LocationData { Name = "Arctic Ocean", Latitude = 70, Longitude = -10 },
    new LocationData { Name = "Sea of Kara", Latitude = 80, Longitude = 0 },
    new LocationData { Name = "Greenland Sea", Latitude = 65, Longitude = -30 },
    new LocationData { Name = "Indian Ocean Southeast", Latitude = -45, Longitude = 45 },
    new LocationData { Name = "Central North Indian Ocean", Latitude = -10, Longitude = 75 },
    new LocationData { Name = "East China Sea", Latitude = 30, Longitude = 120 },
    new LocationData { Name = "South China Sea", Latitude = 15, Longitude = 110 },
    new LocationData { Name = "East Indian Ocean", Latitude = -10, Longitude = 100 },
    new LocationData { Name = "North West Atlantic", Latitude = 50, Longitude = -40 },
    new LocationData { Name = "Central Atlantic South", Latitude = -5, Longitude = -30 },
    new LocationData { Name = "Equatorial Atlantic East", Latitude = 0, Longitude = -15 },
    new LocationData { Name = "Central Atlantic North", Latitude = 30, Longitude = -50 },
    new LocationData { Name = "Southwest Indian Ocean", Latitude = -25, Longitude = 60 },
    new LocationData { Name = "Red Sea", Latitude = 10, Longitude = 40 },
    new LocationData { Name = "North West Atlantic", Latitude = 40, Longitude = -70 },
    new LocationData { Name = "South Pacific Southeast", Latitude = -20, Longitude = -100 },
    new LocationData { Name = "Indian Ocean East", Latitude = -15, Longitude = 120 },
    new LocationData { Name = "Tasman Sea", Latitude = -45, Longitude = 150 },
    new LocationData { Name = "Western Pacific", Latitude = 20, Longitude = 160 },
    new LocationData { Name = "South Pacific Southeast", Latitude = -30, Longitude = -150 },
    new LocationData { Name = "Atlantic South", Latitude = 5, Longitude = -20 },
    new LocationData { Name = "Atlantic Northeast", Latitude = 25, Longitude = -30 },
    new LocationData { Name = "Central Pacific", Latitude = -5, Longitude = -120 },
    new LocationData { Name = "Pacific East", Latitude = -10, Longitude = -80 },
    new LocationData { Name = "Japan Sea", Latitude = 45, Longitude = 135 },
    new LocationData { Name = "Coral Sea", Latitude = 30, Longitude = 150 },
    new LocationData { Name = "South Atlantic", Latitude = -50, Longitude = -10 },
    new LocationData { Name = "North Sea", Latitude = 60, Longitude = 20 },
    new LocationData { Name = "Arctic Ocean East", Latitude = 55, Longitude = 20 },
    new LocationData { Name = "Southern Atlantic", Latitude = -70, Longitude = 120 },
    new LocationData { Name = "South Pacific Southwest", Latitude = -10, Longitude = -180 },
    new LocationData { Name = "Arctic Ocean Northeast", Latitude = 80, Longitude = -90 }
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
                var url = $"{_weatherApiUrl}?latitude={latitudes}&longitude={longitudes}&current_weather=true&format=json";

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

