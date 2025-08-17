document.addEventListener('DOMContentLoaded', function () {
    // --- TODO TU CÓDIGO DEBE ESTAR AQUÍ DENTRO ---

    const toggleBtn = document.getElementById('chatbot-toggle-btn');
    const chatWindow = document.getElementById('chatbot-window');
    const closeBtn = document.getElementById('chatbot-close-btn');
    const messagesContainer = document.getElementById('chatbot-messages');
    const sendBtn = document.getElementById('chatbot-send-btn');
    const input = document.getElementById('chatbot-input');

    // Función para abrir/cerrar
    function toggleChatWindow() {
        chatWindow.classList.toggle('d-none');        
    }

    // --- Asignación de eventos ---
    toggleBtn.addEventListener('click', toggleChatWindow);
    closeBtn.addEventListener('click', toggleChatWindow);

    // --- Lógica de mensajes ---
    function addMessage(text, sender) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message ${sender}-message`;

        if (sender === 'bot') {
            messageDiv.innerHTML = `
            <img src="/images/logoPizzeriaLaQuerencia.jpg" alt="Bot" style="width: 25px; height: 25px; border-radius: 50%; margin-right: 8px; vertical-align: middle;">
            <span>${text}</span>
            `;
        } else {
            messageDiv.textContent = text;
        }
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
            e.preventDefault(); // Añadir preventDefault aquí también
            askBot();
        }
    });

    // Aseguramos el estado inicial
    chatWindow.classList.add('chatbot-hidden');

}); // <-- ESTA ES LA LLAVE DE CIERRE QUE DEBE ESTAR AL FINAL