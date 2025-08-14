document.addEventListener('DOMContentLoaded', function () {
    const toggleBtn = document.getElementById('chatbot-toggle-btn');
    const chatWindow = document.getElementById('chatbot-window');
    const messagesContainer = document.getElementById('chatbot-messages');
    const sendBtn = document.getElementById('chatbot-send-btn');
    const input = document.getElementById('chatbot-input');

    toggleBtn.addEventListener('click', () => {
        chatWindow.classList.toggle('d-none');
    });

    function addMessage(text, sender) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message ${sender}-message`;
        messageDiv.textContent = text;
        messagesContainer.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    async function askBot() {
        const question = input.value.trim();
        if (question === "") return;

        addMessage(question, 'user');
        input.value = "";

        try {
            const response = await fetch('/api/chatbot/ask', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ question: question })
            });

            if (!response.ok) {
                throw new Error(`Error del servidor: ${response.status}`);
            }

            const data = await response.json();
            addMessage(data.answer, 'bot');

        } catch (error) {
            console.error('Error al contactar al bot:', error);
            addMessage('Lo siento, no puedo responder en este momento.', 'bot');
        }
    }

    sendBtn.addEventListener('click', askBot);
    input.addEventListener('keydown', (e) => {
        if (e.key === 'Enter') {
            askBot();
        }
    });
});