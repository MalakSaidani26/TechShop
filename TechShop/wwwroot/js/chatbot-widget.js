// ChatBot Widget - TechShop
(function() {
    'use strict';

    // Configuration
    const CONFIG = {
        apiUrl: '/api/chatbot/message',
        clearUrl: '/api/chatbot/clear-history',
        healthUrl: '/api/chatbot/health'
    };

    // État de la conversation
    const state = {
        isOpen: false,
        isLoading: false,
        messages: []
    };

    // Initialiser le widget
    function init() {
        // Vérifier la disponibilité du service
        checkServiceHealth();

        // Créer l'interface
        createChatWidget();

        // Attacher les événements
        attachEventListeners();
    }

    // Vérifier la santé du service
    async function checkServiceHealth() {
        try {
            const response = await fetch(CONFIG.healthUrl);
            if (!response.ok) {
                console.warn('ChatBot service not available');
            }
        } catch (error) {
            console.warn('ChatBot service health check failed:', error);
        }
    }

    // Créer l'interface du chatbot
    function createChatWidget() {
        const widgetHTML = `
            <div id="chatbot-widget" class="chatbot-widget">
                <div id="chatbot-bubble" class="chatbot-bubble" title="Ouvrir le chat">
                    <i class="fas fa-comments"></i>
                    <span class="chatbot-notification" style="display:none;">1</span>
                </div>

                <div id="chatbot-container" class="chatbot-container" style="display:none;">
                    <div class="chatbot-header">
                        <h3>Support TechShop</h3>
                        <div class="chatbot-controls">
                            <button id="clear-chat-btn" class="chatbot-btn-icon" title="Effacer la conversation">
                                <i class="fas fa-trash-alt"></i>
                            </button>
                            <button id="close-chat-btn" class="chatbot-btn-icon" title="Fermer">
                                <i class="fas fa-times"></i>
                            </button>
                        </div>
                    </div>

                    <div id="chatbot-messages" class="chatbot-messages"></div>

                    <div class="chatbot-input-area">
                        <input 
                            type="text" 
                            id="chatbot-input" 
                            class="chatbot-input" 
                            placeholder="Posez votre question..."
                            maxlength="1000"
                        />
                        <button id="send-btn" class="chatbot-send-btn" title="Envoyer">
                            <i class="fas fa-paper-plane"></i>
                        </button>
                    </div>
                </div>
            </div>
        `;

        // Injecter le HTML
        document.body.insertAdjacentHTML('beforeend', widgetHTML);
    }

    // Attacher les événements
    function attachEventListeners() {
        const bubble = document.getElementById('chatbot-bubble');
        const container = document.getElementById('chatbot-container');
        const sendBtn = document.getElementById('send-btn');
        const input = document.getElementById('chatbot-input');
        const closeBtn = document.getElementById('close-chat-btn');
        const clearBtn = document.getElementById('clear-chat-btn');

        // Ouvrir/Fermer
        bubble.addEventListener('click', () => {
            state.isOpen = !state.isOpen;
            container.style.display = state.isOpen ? 'flex' : 'none';
            if (state.isOpen && state.messages.length === 0) {
                showWelcomeMessage();
            }
        });

        closeBtn.addEventListener('click', () => {
            state.isOpen = false;
            container.style.display = 'none';
        });

        // Envoyer le message
        sendBtn.addEventListener('click', sendMessage);
        input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !state.isLoading) {
                sendMessage();
            }
        });

        // Effacer l'historique
        clearBtn.addEventListener('click', async () => {
            if (confirm('Voulez-vous effacer l\'historique de conversation?')) {
                await fetch(CONFIG.clearUrl, { method: 'POST' });
                state.messages = [];
                document.getElementById('chatbot-messages').innerHTML = '';
                showWelcomeMessage();
            }
        });
    }

    // Afficher le message de bienvenue
    function showWelcomeMessage() {
        const messagesDiv = document.getElementById('chatbot-messages');
        const welcomeMsg = document.createElement('div');
        welcomeMsg.className = 'chatbot-message bot-message';
        welcomeMsg.innerHTML = `
            <div class="chatbot-message-content">
                Bonjour! 👋 Bienvenue chez TechShop. Je suis votre assistant virtuel. Comment puis-je vous aider?
            </div>
        `;
        messagesDiv.appendChild(welcomeMsg);
        scrollToBottom();
    }

    // Envoyer un message
    async function sendMessage() {
        const input = document.getElementById('chatbot-input');
        const message = input.value.trim();

        if (!message || state.isLoading) {
            return;
        }

        // Afficher le message utilisateur
        addMessage(message, 'user');
        input.value = '';
        state.isLoading = true;

        try {
            // Envoyer au serveur
            const response = await fetch(CONFIG.apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    message: message,
                    context: ''
                })
            });

            const data = await response.json();

            if (data.success) {
                addMessage(data.response, 'bot');
                state.messages.push({
                    role: 'bot',
                    content: data.response
                });
            } else {
                addMessage('Erreur: ' + (data.error || 'Impossible de traiter votre demande'), 'bot');
            }
        } catch (error) {
            console.error('Error sending message:', error);
            addMessage('Une erreur réseau s\'est produite. Veuillez réessayer.', 'bot');
        } finally {
            state.isLoading = false;
            document.getElementById('chatbot-input').focus();
        }
    }

    // Ajouter un message à la conversation
    function addMessage(text, role) {
        const messagesDiv = document.getElementById('chatbot-messages');
        const messageDiv = document.createElement('div');
        messageDiv.className = `chatbot-message ${role === 'user' ? 'user-message' : 'bot-message'}`;

        const contentDiv = document.createElement('div');
        contentDiv.className = 'chatbot-message-content';
        contentDiv.textContent = text;

        messageDiv.appendChild(contentDiv);
        messagesDiv.appendChild(messageDiv);

        scrollToBottom();
    }

    // Scroller vers le bas
    function scrollToBottom() {
        const messagesDiv = document.getElementById('chatbot-messages');
        setTimeout(() => {
            messagesDiv.scrollTop = messagesDiv.scrollHeight;
        }, 0);
    }

    // Initialiser au chargement
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
