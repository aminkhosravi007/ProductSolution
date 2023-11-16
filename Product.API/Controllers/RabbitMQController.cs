using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Application;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;

        public RabbitMQController()
        {
            _rabbitMQService = new RabbitMQService();
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            try
            {
                _rabbitMQService.SendMessage("myQueue", message);
                return Ok("Message sent successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending message: {ex.Message}");
            }
        }

        [HttpGet("receive")]
        public IActionResult ReceiveMessage()
        {
            try
            {
                _rabbitMQService.ReceiveMessage("myQueue");
                return Ok("Receiving messages...");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error receiving message: {ex.Message}");
            }
        }

        [HttpGet("close")]
        public IActionResult CloseConnection()
        {
            _rabbitMQService.CloseConnection();
            return Ok("Connection closed");
        }
    }
}
