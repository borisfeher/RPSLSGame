using Microsoft.AspNetCore.Mvc;
using RPSLSGame.Models;
using RPSLSGame.Services;

namespace RPSLSGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoiceController : ControllerBase
    {
        private readonly IRandomChoiceService _randomChoiceService;

        public ChoiceController(IRandomChoiceService randomChoiceService)
        {
            _randomChoiceService = randomChoiceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ChoiceModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetRandomChoice()
        {
            try
            {
                var randomChoice = await _randomChoiceService.GetRandomChoiceAsync();
                return Ok(randomChoice);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "Failed to retrieve random choice. Please try again later.");
            }
        }
    }
}