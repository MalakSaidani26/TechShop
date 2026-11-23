using TechShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace TechShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        private readonly ILogger<ChatBotController> _logger;

        private static List<ChatMessage> _conversationHistory = new();

        public ChatBotController(IChatBotService chatBotService, ILogger<ChatBotController> logger)
        {
            _chatBotService = chatBotService;
            _logger = logger;
        }

        [HttpPost("message")]
        public async Task<ActionResult<ChatBotResponse>> SendMessage([FromBody] ChatBotRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Message))
                {
                    return BadRequest(new ChatBotResponse
                    {
                        Success = false,
                        Error = "Le message ne peut pas être vide"
                    });
                }

                // Limiter la longueur du message
                var message = request.Message.Trim();
                if (message.Length > 1000)
                {
                    return BadRequest(new ChatBotResponse
                    {
                        Success = false,
                        Error = "Le message est trop long (maximum 1000 caractères)"
                    });
                }

                // Ajouter le message à l'historique
                _conversationHistory.Add(new ChatMessage { Role = "user", Content = message });

                // Construire le contexte de conversation
                var context = BuildConversationContext();

                // Obtenir la réponse du chatbot
                var response = await _chatBotService.GetChatResponseAsync(message, context);

                // Ajouter la réponse à l'historique
                _conversationHistory.Add(new ChatMessage { Role = "assistant", Content = response });

                // Garder seulement les 10 derniers messages pour l'historique
                if (_conversationHistory.Count > 20)
                {
                    _conversationHistory = _conversationHistory.Skip(_conversationHistory.Count - 20).ToList();
                }

                return Ok(new ChatBotResponse
                {
                    Success = true,
                    Response = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ChatBot controller: {ex.Message}");
                return StatusCode(500, new ChatBotResponse
                {
                    Success = false,
                    Error = "Une erreur serveur s'est produite"
                });
            }
        }

        [HttpPost("clear-history")]
        public ActionResult ClearHistory()
        {
            _conversationHistory.Clear();
            return Ok(new { message = "Historique cleared" });
        }

        [HttpGet("health")]
        public ActionResult<object> Health()
        {
            return Ok(new { status = "ok", connected = true });
        }

        private string BuildConversationContext()
        {
            if (_conversationHistory.Count <= 2)
                return string.Empty;

            // Construire un contexte avec les derniers messages (sans le dernier qui est le message actuel)
            var context = string.Join("\n", _conversationHistory
                .Take(_conversationHistory.Count - 1)
                .Select(m => $"{(m.Role == "user" ? "Client" : "Assistant")}: {m.Content}"));

            return context;
        }
    }

    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
