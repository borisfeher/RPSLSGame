using Microsoft.AspNetCore.Mvc;
using RPSLSGame.Models;
using RPSLSGame.Services;

namespace RPSLSGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoicesController : ControllerBase
    {
        private readonly IChoiceService _choiceService;

        public ChoicesController(IChoiceService choiceService)
        {
            _choiceService = choiceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ChoiceModel>), StatusCodes.Status200OK)]
        public IActionResult GetChoices()
        {
            var choices = _choiceService.GetChoices();
            return Ok(choices);
        }
    }
}