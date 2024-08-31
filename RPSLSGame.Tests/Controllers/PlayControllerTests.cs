using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RPSLSGame.Controllers;
using RPSLSGame.Models;
using RPSLSGame.Services;

namespace RPSLSGame.Tests.Controllers
{
    public class PlayControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IGameResultService> _gameResultServiceMock;
        private readonly PlayController _playController;

        public PlayControllerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _gameResultServiceMock = new Mock<IGameResultService>();
            _playController = new PlayController(_gameServiceMock.Object, _gameResultServiceMock.Object);
        }

        [Fact]
        public async Task Play_ReturnsOk_WithValidChoices()
        {
            // Arrange
            var playerChoice = new PlayerChoice { ChoiceId = 1 }; // Rock
            _gameServiceMock.Setup(s => s.GetComputerChoiceAsync()).ReturnsAsync(2); // Paper
            _gameServiceMock.Setup(s => s.GetChoiceName(1)).Returns("rock");
            _gameServiceMock.Setup(s => s.GetChoiceName(2)).Returns("paper");
            _gameServiceMock.Setup(s => s.DetermineWinner("rock", "paper")).Returns("lose");

            // Act
            var result = await _playController.Play(playerChoice);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GamePlayResponse>(okResult.Value);

            Assert.NotNull(response);
            Assert.Equal("lose", response.Results);
            Assert.Equal("rock", response.Player.Name);
            Assert.Equal("paper", response.Computer.Name);
        }

        [Fact]
        public async Task Play_ReturnsBadRequest_ForInvalidChoiceId()
        {
            // Arrange
            var playerChoice = new PlayerChoice { ChoiceId = 6 }; // Invalid ID

            // Act
            var result = await _playController.Play(playerChoice);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid choice. Please select a value between 1 and 5.", badRequestResult.Value);
        }

        [Fact]
        public async Task Play_Returns503_WhenHttpRequestExceptionThrown()
        {
            // Arrange
            var playerChoice = new PlayerChoice { ChoiceId = 1 }; // Rock
            _gameServiceMock.Setup(s => s.GetComputerChoiceAsync()).ThrowsAsync(new HttpRequestException());

            // Act
            var result = await _playController.Play(playerChoice);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusCodeResult.StatusCode);
            Assert.Equal("Failed to retrieve random number. Please try again later.", statusCodeResult.Value);
        }

        [Fact]
        public void GetRecentResults_ReturnsOk_WithListOfGameResults()
        {
            // Arrange
            var recentResults = new List<GameResult>
            {
                new GameResult
                {
                    PlayerChoice = "rock",
                    ComputerChoice = "scissors",
                    Result = "win",
                    TimeStamp = DateTime.Now
                },
                new GameResult
                {
                    PlayerChoice = "paper",
                    ComputerChoice = "rock",
                    Result = "win",
                    TimeStamp = DateTime.Now
                }
            };

            _gameResultServiceMock.Setup(s => s.GetRecentResults(It.IsAny<int>())).Returns(recentResults);

            // Act
            var result = _playController.GetRecentResults();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResults = Assert.IsType<List<GameResult>>(okResult.Value);
            Assert.Equal(recentResults.Count, returnedResults.Count);
        }

        [Fact]
        public void GetRecentResults_Returns500_WhenExceptionThrown()
        {
            // Arrange
            _gameResultServiceMock.Setup(s => s.GetRecentResults(It.IsAny<int>())).Throws(new Exception());

            // Act
            var result = _playController.GetRecentResults();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while retrieving the recent results.", statusCodeResult.Value);
        }

        [Fact]
        public void ResetResults_ReturnsOk()
        {
            // Arrange
            _gameResultServiceMock.Setup(s => s.ResetResults());

            // Act
            var result = _playController.ResetResults();

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _gameResultServiceMock.Verify(s => s.ResetResults(), Times.Once);
        }
    }
}