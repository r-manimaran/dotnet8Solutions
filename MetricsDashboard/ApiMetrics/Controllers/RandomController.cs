using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiMetrics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RandomController : ControllerBase
    {
        private readonly ILogger<RandomController> _logger;
        public RandomController(ILogger<RandomController> logger)
        {
            _logger = logger;
        }

        [HttpGet("rnd")]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Random request received");
            int delay = new Random().Next(1000, 5000);
            await Task.Delay(delay);

            int exceptionChance = new Random().Next(1, 11);
            if (exceptionChance == 1)
            {
                _logger.LogError("Unhandled Exception thrown");
                throw new Exception("Random exception");
            }

            int result = new Random().Next(1, 6);
            switch (result)
            {
                case 1:
                    _logger.LogWarning("Bad Request");
                    return BadRequest(new { message = "Bad Request" });
                case 2:
                    _logger.LogWarning("Not Found");
                    return NotFound(new { message = "Not Found" });
                case 3:
                    _logger.LogWarning("Unauthorized");
                    return StatusCode(500, new { message = "Internal Server Error" });                
                default:
                    return Ok(new { message = "Success everrything is fine" });
            }
            
        }
       
    }
}