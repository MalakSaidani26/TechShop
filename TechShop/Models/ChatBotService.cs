using System.Net.Http.Json;
using System.Text.Json;

namespace TechShop.Models
{
    public interface IChatBotService
    {
        Task<string> GetChatResponseAsync(string userMessage, string conversationContext = "");
    }

    public class ChatBotRequest
    {
        public string Message { get; set; }
        public string Context { get; set; }
    }

    public class ChatBotResponse
    {
        public string Response { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class ChatBotService : IChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ChatBotService> _logger;

        public ChatBotService(HttpClient httpClient, IConfiguration configuration, ILogger<ChatBotService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetChatResponseAsync(string userMessage, string conversationContext = "")
        {
            try
            {
                var apiKey = _configuration["OpenAI:ApiKey"];
                var model = _configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";

                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("OpenAI API key not configured");
                    return "Je suis désolé, le service de chat n'est pas correctement configuré. Veuillez contacter le support.";
                }

                // Construire le contexte système pour TechShop
                var systemPrompt = GetSystemPrompt(conversationContext);

                // Créer la demande OpenAI
                var request = new OpenAIRequest
                {
                    Model = model,
                    Messages = new[]
                    {
                        new ChatMessage { Role = "system", Content = systemPrompt },
                        new ChatMessage { Role = "user", Content = userMessage }
                    },
                    Temperature = 0.7,
                    MaxTokens = 500
                };

                // Préparer les headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // Envoyer la demande
                var response = await _httpClient.PostAsJsonAsync(
                    "https://api.openai.com/v1/chat/completions",
                    request
                );

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"OpenAI API error: {response.StatusCode} - {errorContent}");
                    return "Une erreur est survenue lors de la communication avec le service de chat. Veuillez réessayer.";
                }

                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<OpenAIResponse>(content);

                if (jsonResponse?.Choices?.Count > 0)
                {
                    return jsonResponse.Choices[0].Message.Content;
                }

                return "Impossible de traiter votre demande. Veuillez réessayer.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"ChatBot service error: {ex.Message}");
                return "Une erreur technique s'est produite. Veuillez réessayer ou contacter le support.";
            }
        }

        private string GetSystemPrompt(string context)
        {
            var basePrompt = @"Tu es un assistant de support client pour TechShop, une boutique en ligne de produits technologiques.

Ton rôle est d'aider les clients avec :
1. Les questions sur les produits (spécifications, prix, disponibilité)
2. Les problèmes de livraison et de commande
3. Les retours et remboursements
4. L'aide à la navigation sur le site
5. Les demandes générales de support

Directives :
- Réponds toujours en français, à moins que le client n'utilise une autre langue
- Sois courtois, professionnel et serviable
- Si tu ne sais pas répondre, propose au client de contacter le support directement : support@techshop.local
- Pour les commandes spécifiques, demande le numéro de commande
- Fournis des réponses concises et claires
- Si c'est un problème urgent, recommande de contacter directement le support par email ou téléphone";

            if (!string.IsNullOrEmpty(context))
            {
                basePrompt += $"\n\nContexte de la conversation :\n{context}";
            }

            return basePrompt;
        }
    }

    // Classes pour la sérialisation JSON
    public class OpenAIRequest
    {
        [System.Text.Json.Serialization.JsonPropertyName("model")]
        public string Model { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("messages")]
        public ChatMessage[] Messages { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }
    }

    public class OpenAIResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        [System.Text.Json.Serialization.JsonPropertyName("message")]
        public ChatMessage Message { get; set; }
    }

    public class ChatMessage
    {
        [System.Text.Json.Serialization.JsonPropertyName("role")]
        public string Role { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
