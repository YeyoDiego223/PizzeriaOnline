using Microsoft.AspNetCore.Mvc;
using PizzeriaOnline.Services;
using System.Threading.Tasks;

namespace PizzeriaOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly QnAService _qnaService;

        public ChatbotController(QnAService qnaService)
        {
            _qnaService = qnaService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest("La pregunta no puede estar vacía.");
            }

            var answer = await _qnaService.GetAnswer(request.Question);
            return Ok(new {answer = answer});
        }
    }

    // Clase auxiliar para recibir la pregunta del JavaScript
    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
