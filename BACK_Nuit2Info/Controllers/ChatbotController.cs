using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using dotenv.net;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string context = "Tu es un expert en oceanologie, en climatologie, en anatomie et biologie humaine et sur le jeu video Monkey Island. Tu travailles sur une application qui compare des elements oceaniques avec des parties du corps humain dans le but de marquer l importance de certains phenomenes et sensibiliser le public a la protection de nos oceans. Tu doit imperativement rediger des questions et des reponses liees a des comparaisons entre le corps humain et des phenomenes oceanique/climatiques pour renseigner les utilisateurs. L'analogie entre le corps humain et les oceans doit toujours apparaitre dans ton contenu, voici ce qui t es demande :";

    public ChatbotController()
    {
        // API KEY
        DotEnv.Load();
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(_apiKey))
        {
            Console.WriteLine("La clé API OpenAI est manquante ou invalide.");
            throw new Exception("La clé API OpenAI est manquante. Vérifiez vos variables d'environnement.");
        }

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        Console.WriteLine("En-têtes HTTP : " + string.Join(", ", _httpClient.DefaultRequestHeaders));
    }

    // QUESTION GENERATION
    [HttpPost("generate-questions")]
    public async Task<IActionResult> GenerateQuestions([FromBody] string info)
    {
        var messages = new[]
        {
            new
            {
                role = "system",
                content = context
            },
            new
            {
                role = "user",
                content = $"Voici une information : {info}. Genere 3 questions differentes, pertinentes, serieuses et interessantes que quelqu un pourrait poser pour obtenir des renseignements ou des informations complementaires suite a cette information. Formules-les de facon curieuse et serieuse sur le sujet concerne. Chacune des questions doit confronter le corps humains et les oceans et ne doit pa sdepasser 30 tokens. Tu dois strictement envoyer chaque question en suivant le format suivant sans ecrire de .  : NumeroDeQuestionQuestion ?"
            }
        };

        var payload = new
        {
            model = "gpt-4o-mini",
            messages = messages,
            max_tokens = 500,
            temperature = 1
        };

        try
        {
            var response = await CallOpenAIAPI(payload);
            Console.WriteLine("Réponse brute de l'API : " + response);

            if (response == null)
            {
                Console.WriteLine("Réponse de l'API est nulle.");
                return StatusCode(500, "Une erreur s'est produite lors de l'appel à l'API OpenAI.");
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'appel à l'API : {ex.Message}");
            return StatusCode(500, "Erreur interne serveur.");
        }
    }

    // ANSWER QUESTIONS
    [HttpPost("answer-questions")]
    public async Task<IActionResult> AnswerQuestions([FromBody] string question, [FromQuery] bool pirateMode)
    {
        var messages = pirateMode
            ? new[]
            {
                new
                {
                    role = "system",
                    content = context
                },
                new
                {
                    role = "user",
                    content = $"Reponds a la question suivante: {question}. Tu dois strictement repondre en moins de 150 tokens a cette question de facon claire, coherante et simplement comprehensible tout en ajoutant de l'humour lié a diverses references du jeu Monkey Island sans rien ajouter d'autre."
                }
            }
            : new[]
            {
                new
                {
                    role = "system",
                    content = context
                },
                new
                {
                    role = "user",
                    content = $"Reponds a la question suivante: {question}. Tu dois strictement repondre en moins de 150 tokens a cette question de facon claire, coherante, concise et educative et sans rien ajouter d'autre."
                }
            };

        var payload = new
        {
            model = "gpt-4o-mini",
            messages = messages,
            max_tokens = 500,
            temperature = 1
        };

        try
        {
            var response = await CallOpenAIAPI(payload);
            Console.WriteLine("Réponse brute de l'API : " + response);

            if (response == null)
            {
                Console.WriteLine("Réponse de l'API est nulle.");
                return StatusCode(500, "Une erreur s'est produite lors de l'appel à l'API OpenAI.");
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'appel à l'API : {ex.Message}");
            return StatusCode(500, "Erreur interne serveur.");
        }
    }

    // Call OpenAI API
    private async Task<string> CallOpenAIAPI(object payload)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Réponse obtenue de l'API : " + result);

                var parsedResult = JsonDocument.Parse(result);
                return parsedResult.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            }
            Console.WriteLine($"Erreur API OpenAI : {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'appel à l'API OpenAI : {ex.Message}");
        }

        return null;
    }
}
