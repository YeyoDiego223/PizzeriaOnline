using Azure;
using Azure.AI.Language.QuestionAnswering;
using System;
using System.Threading.Tasks;

namespace PizzeriaOnline.Services
{
    public class QnAService
    {
        private readonly QuestionAnsweringClient _client;
        private readonly string _projectName;

        public QnAService(IConfiguration configuration)
        {
            Uri endpoint = new Uri(configuration["AzureAI:Endpoint"]);
            AzureKeyCredential credential = new AzureKeyCredential(configuration["AzureAI:Key"]);

            _projectName = "PizzeriaFAQ";

            _client = new QuestionAnsweringClient(endpoint, credential);
        }

        public async Task<string> GetAnswer(string question)
        {
            try
            {
                var project = new QuestionAnsweringProject(_projectName, "production");
                Response<AnswersResult> response = await _client.GetAnswersAsync(question, project);

                foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                {
                    if (answer.Confidence > 0.5)
                    {
                        return answer.Answer;
                    }
                }

                return "Vaya, esa es una pregunta muy específica. Para darte la mejor atención, por favor contáctanos directamente por WhatsApp y uno de nuestros expertos te atenderá. Diriguete a la sección de Contacto";
            }
            catch (Exception ex)
            {
                return "Lo siento, hubo un problema al conectar con nuestro asistente.";
            }
        }
    }
}