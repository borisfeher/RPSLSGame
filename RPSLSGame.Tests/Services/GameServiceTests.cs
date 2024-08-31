using Microsoft.Extensions.Configuration;
using Moq;
using RPSLSGame.Services;
using System.Net;

namespace RPSLSGame.Tests.Services
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public GameServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationMock = new Mock<IConfiguration>();

            // Setup configuration mock
            _configurationMock.Setup(config => config["RandomNumberApi:Url"])
                              .Returns("https://codechallenge.boohma.com/random");

            // Setup mock HTTP response
            var mockHttpMessageHandler = new MockHttpMessageHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"random_number\": 3}")
            });

            var httpClient = new HttpClient(mockHttpMessageHandler);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _gameService = new GameService(_httpClientFactoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public void GetChoiceName_ValidChoiceId_ReturnsCorrectChoiceName()
        {
            // Arrange
            var choiceId = 1;

            // Act
            var result = _gameService.GetChoiceName(choiceId);

            // Assert
            Assert.Equal("rock", result);
        }

        [Fact]
        public void DetermineWinner_PlayerWins_ReturnsWin()
        {
            // Arrange
            var playerChoice = "rock";
            var computerChoice = "scissors";

            // Act
            var result = _gameService.DetermineWinner(playerChoice, computerChoice);

            // Assert
            Assert.Equal("win", result);
        }

        [Fact]
        public async Task GetComputerChoiceAsync_ReturnsValidChoice()
        {
            // Act
            var result = await _gameService.GetComputerChoiceAsync();

            // Assert
            Assert.InRange(result, 1, 5);
        }
    }
}