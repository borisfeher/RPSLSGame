using Microsoft.AspNetCore.Mvc;
using RPSLSGame.Models;
using RPSLSGame.Services;

namespace RPSLSGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayController : ControllerBase
    {
        private const int MinChoiceId = 1;
        private const int MaxChoiceId = 5;
        private readonly IGameService _gameService;
        private readonly IGameResultService _gameResultService;

        public PlayController(IGameService gameService, IGameResultService gameResultService)
        {
            _gameService = gameService;
            _gameResultService = gameResultService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Play([FromBody] PlayerChoice playerChoice)
        {
            if (playerChoice == null || playerChoice.ChoiceId < MinChoiceId || playerChoice.ChoiceId > MaxChoiceId)
            {
                return BadRequest($"Invalid choice. Please select a value between {MinChoiceId} and {MaxChoiceId}.");
            }

            try
            {
                int computerChoiceId = await _gameService.GetComputerChoiceAsync();
                string computerChoice = _gameService.GetChoiceName(computerChoiceId);
                string playerSelection = _gameService.GetChoiceName(playerChoice.ChoiceId);

                string result = _gameService.DetermineWinner(playerSelection, computerChoice);

                var playerChoiceData = new ChoiceData
                {
                    Name = playerSelection,
                    Image = $"/images/{playerSelection.ToLower()}.png"
                };

                var computerChoiceData = new ChoiceData
                {
                    Name = computerChoice,
                    Image = $"/images/{computerChoice.ToLower()}.png"
                };

                var gameResult = new GameResult
                {
                    PlayerChoice = playerSelection,
                    ComputerChoice = computerChoice,
                    Result = result,
                    TimeStamp = DateTime.Now
                };

                _gameResultService.AddResult(gameResult);

                var response = new GamePlayResponse
                {
                    Results = result,
                    Player = playerChoiceData,
                    Computer = computerChoiceData
                };

                return Ok(response);
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "Failed to retrieve random number. Please try again later.");
            }
        }

        [HttpGet("recent")]
        [ProducesResponseType(typeof(IEnumerable<GameResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetRecentResults()
        {
            try
            {
                var results = _gameResultService.GetRecentResults();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the recent results.");
            }
        }

        [HttpPost("reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ResetResults()
        {
            _gameResultService.ResetResults();
            return Ok();
        }
    }
}