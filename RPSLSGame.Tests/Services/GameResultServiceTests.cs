using RPSLSGame.Models;
using RPSLSGame.Services;

namespace RPSLSGame.Tests.Services
{
    public class GameResultServiceTests
    {
        private readonly GameResultService _gameResultService;

        public GameResultServiceTests()
        {
            _gameResultService = new GameResultService();
        }

        [Fact]
        public void AddResult_ShouldAddResultToList()
        {
            // Arrange
            var result = new GameResult
            {
                PlayerChoice = "rock",
                ComputerChoice = "scissors",
                Result = "win",
                TimeStamp = DateTime.Now
            };

            // Act
            _gameResultService.AddResult(result);
            var results = _gameResultService.GetRecentResults().ToList();

            // Assert
            Assert.Single(results);
            Assert.Equal(result, results[0]);
        }

        [Fact]
        public void GetRecentResults_ShouldReturnResultsInDescendingOrder()
        {
            // Arrange
            var result1 = new GameResult
            {
                PlayerChoice = "rock",
                ComputerChoice = "scissors",
                Result = "win",
                TimeStamp = DateTime.Now.AddMinutes(-10)
            };

            var result2 = new GameResult
            {
                PlayerChoice = "paper",
                ComputerChoice = "rock",
                Result = "win",
                TimeStamp = DateTime.Now.AddMinutes(-5)
            };

            var result3 = new GameResult
            {
                PlayerChoice = "scissors",
                ComputerChoice = "paper",
                Result = "win",
                TimeStamp = DateTime.Now
            };

            _gameResultService.AddResult(result1);
            _gameResultService.AddResult(result2);
            _gameResultService.AddResult(result3);

            // Act
            var results = _gameResultService.GetRecentResults().ToList();

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(result3, results[0]);
            Assert.Equal(result2, results[1]);
            Assert.Equal(result1, results[2]);
        }

        [Fact]
        public void ResetResults_ShouldClearAllResults()
        {
            // Arrange
            var result = new GameResult
            {
                PlayerChoice = "rock",
                ComputerChoice = "scissors",
                Result = "win",
                TimeStamp = DateTime.Now
            };

            _gameResultService.AddResult(result);

            // Act
            _gameResultService.ResetResults();
            var results = _gameResultService.GetRecentResults().ToList();

            // Assert
            Assert.Empty(results);
        }
    }
}