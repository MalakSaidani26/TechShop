using System.Net.Http.Json;
using System.Text.Json;

namespace TechShop.Models
{
    /// <summary>
    /// Mock ChatBot Service pour les tests sans API OpenAI valide
    /// Répond des messages pré-programmés
    /// </summary>
    public class MockChatBotService : IChatBotService
    {
        private readonly ILogger<MockChatBotService> _logger;
        private static readonly Dictionary<string, string> _responses = new()
        {
            { "bonjour", "Bonjour! 👋 Bienvenue chez TechShop. Comment puis-je vous aider?" },
            { "prix", "Nos prix sont très compétitifs. Consultez notre catalogue pour voir les tarifs actuels." },
            { "livraison", "Nous livrons partout en Tunisie . Les délais sont généralement de 3-5 jours ouvrables." },
            { "retour", "Vous avez 14 jours pour retourner un produit après réception. Contactez notre support pour plus d'infos." },
            { "paiement", "Nous acceptons les cartes bancaires, PayPal et les virements bancaires." },
            { "produit", "Nous vendons des ordinateurs portables, de bureau, des accessoires et bien plus!" },
            { "commande", "Pour suivre votre commande, veuillez contacter notre support avec votre numéro de commande." },
            { "contact", "Vous pouvez nous contacter à support@techshop.local ou via ce chat." },
            { "default", "Je suis une version de test du chatbot. Pour de vraies réponses, vérifiez que votre compte OpenAI a des crédits disponibles. 😊" }
        };

        public MockChatBotService(ILogger<MockChatBotService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetChatResponseAsync(string userMessage, string conversationContext = "")
        {
            // Simuler une latence réseau
            await Task.Delay(500 + Random.Shared.Next(500));

            var message = userMessage.ToLower().Trim();
            _logger.LogInformation($"Mock ChatBot received: {message}");

            // Chercher une réponse correspondante
            foreach (var (key, response) in _responses)
            {
                if (message.Contains(key))
                {
                    _logger.LogInformation($"Mock ChatBot responding with: {response}");
                    return response;
                }
            }

            // Réponse par défaut
            return _responses["default"];
        }
    }

    /// <summary>
    /// Wrapper utilisant soit ChatBotService, soit MockChatBotService
    /// selon la configuration
    /// </summary>
    public class HybridChatBotService : IChatBotService
    {
        private readonly IChatBotService _innerService;
        private readonly ILogger<HybridChatBotService> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public HybridChatBotService(IChatBotService innerService, ILogger<HybridChatBotService> logger, ILoggerFactory loggerFactory)
        {
            _innerService = innerService;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public async Task<string> GetChatResponseAsync(string userMessage, string conversationContext = "")
        {
            try
            {
                return await _innerService.GetChatResponseAsync(userMessage, conversationContext);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("429") || ex.Message.Contains("quota"))
            {
                _logger.LogWarning($"OpenAI API quota exceeded, falling back to mock: {ex.Message}");
                // Fallback to mock si quota épuisé
                var mockLogger = _loggerFactory.CreateLogger<MockChatBotService>();
                var mockService = new MockChatBotService(mockLogger);
                return await mockService.GetChatResponseAsync(userMessage, conversationContext);
            }
        }
    }
}
