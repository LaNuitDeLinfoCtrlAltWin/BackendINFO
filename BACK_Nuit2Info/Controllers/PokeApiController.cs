using PokeApiNet;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BACK_Nuit2Info.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokeApiController : ControllerBase
    {
        private readonly PokeApiClient pokeClient;

        private static List<string> PokemonNames = new List<string>();

        public PokeApiController()
        {
            pokeClient = new PokeApiClient();
        }
        private static async Task LoadPokemonNamesAsync()
        {
            if (PokemonNames.Count == 0)
            {
                string apiUrl = "https://pokeapi.co/api/v2/pokemon?limit=200";

                using HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(jsonResponse);
                var results = jsonDocument.RootElement.GetProperty("results");

                foreach (var result in results.EnumerateArray())
                {
                    string name = result.GetProperty("name").GetString();
                    PokemonNames.Add(name);
                }
            }
        }

        [HttpGet(Name = "GetPokemon")]
        public async Task<string> GetPokemon()
        {
            try
            {
                await LoadPokemonNamesAsync();

                Random random = new Random();
                int randomIndex = random.Next(PokemonNames.Count);
                string randomName = PokemonNames[randomIndex];

                Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(randomName);

                if (poke == null || poke.Sprites == null)
                {
                    throw new Exception("Les données du Pokémon sont nulles ou incomplètes.");
                }
                var jsonObj = new
                {
                    nom = randomName,
                    img = poke.Sprites.FrontDefault
                };

                return JsonSerializer.Serialize(jsonObj);
            }
            catch (Exception ex)
            {
                var errorObj = new
                {
                    error = "Une erreur s'est produite lors de la récupération du Pokémon.",
                    details = ex.Message
                };
                return JsonSerializer.Serialize(errorObj);
            }
        }
    }
}
