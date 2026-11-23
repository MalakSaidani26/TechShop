using System.Net.Http.Json;
using System.Text.Json;

namespace TechShop.Models
{
    /// <summary>
    /// Service ChatBot utilisant Deepseek V4 Pro via TeamoRouter API
    /// API gratuite: https://api.teamorouter.com/v1
    /// Clé gratuite: sk-teamo-74defe341ee018ce945b1b48bee9f67aaf9582e24a2d91b0
    /// </summary>
    public class DeepseekChatBotService : IChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeepseekChatBotService> _logger;

        // Configuration TeamoRouter Deepseek
        private const string TEAMO_API_URL = "https://api.teamorouter.com/v1/chat/completions";
        private const string DEEPSEEK_MODEL = "deepseek-v4-pro-free";

        public DeepseekChatBotService(HttpClient httpClient, IConfiguration configuration, ILogger<DeepseekChatBotService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GetChatResponseAsync(string userMessage, string conversationContext = "")
        {
            try
            {
                var apiKey = _configuration["Deepseek:ApiKey"];

                if (string.IsNullOrEmpty(apiKey))
                {
                    _logger.LogError("Deepseek API key not configured");
                    return "Je suis désolé, le service de chat n'est pas correctement configuré. Veuillez contacter le support.";
                }

                // Construire le contexte système pour TechShop
                var systemPrompt = GetSystemPrompt(conversationContext);

                // Créer la demande compatible avec Deepseek/OpenAI format
                var request = new ChatCompletionRequest
                {
                    Model = DEEPSEEK_MODEL,
                    Messages = new[]
                    {
                        new ChatMessage { Role = "system", Content = systemPrompt },
                        new ChatMessage { Role = "user", Content = userMessage }
                    },
                    Temperature = 0.7,
                    MaxTokens = 500,
                    TopP = 0.95
                };

                // Préparer les headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // Envoyer la demande à TeamoRouter
                _logger.LogInformation($"Sending request to Deepseek via TeamoRouter: {request.Messages.Last().Content}");

                var response = await _httpClient.PostAsJsonAsync(
                    TEAMO_API_URL,
                    request
                );

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Deepseek API error: {response.StatusCode} - {errorContent}");
                    return "Une erreur est survenue lors de la communication avec le service de chat. Veuillez réessayer.";
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Deepseek response: {content}");

                var jsonResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (jsonResponse?.Choices?.Count > 0)
                {
                    var result = jsonResponse.Choices[0].Message.Content;
                    _logger.LogInformation($"ChatBot response: {result}");
                    return result;
                }

                return "Impossible de traiter votre demande. Veuillez réessayer.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Deepseek ChatBot service error: {ex.Message}");
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

    /// <summary>
    /// Classe de requête compatible avec Deepseek/OpenAI format
    /// </summary>
    public class ChatCompletionRequest
    {
        [System.Text.Json.Serialization.JsonPropertyName("model")]
        public string Model { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("messages")]
        public ChatMessage[] Messages { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("top_p")]
        public double TopP { get; set; }
    }

    /// <summary>
    /// Classe de réponse compatible avec Deepseek/OpenAI format
    /// </summary>
    public class ChatCompletionResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("usage")]
        public Usage Usage { get; set; }
    }

    public class Usage
    {
        [System.Text.Json.Serialization.JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
