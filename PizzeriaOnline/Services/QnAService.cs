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

                return "Lo siento, no he podido encontrar una respuesta a tu pregunta. Intenta reformularla.";
            }
            catch (Exception ex)
            {
                return "Lo siento, hubo un problema al conectar con nuestro asistente.";
            }
        }
    }
}