using PokeApiNet;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace BACK_Nuit2Info.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokeApiController : ControllerBase
    {
        private readonly PokeApiClient pokeClient;

        public PokeApiController()
        {
            pokeClient = new PokeApiClient();
        }

        [HttpGet(Name = "GetPokemon")]
        public async Task<string> GetPokemon()
        {
            try
            {
                string name = "ho-oh";
                Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(name);

                if (poke == null || poke.Sprites == null)
                {
                    throw new Exception("Les données du Pokémon sont nulles ou incomplètes.");
                }

                var jsonObj = new
                {
                    nom = name,
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
