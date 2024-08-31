using Microsoft.AspNetCore.Mvc;
using Moq;
using RPSLSGame.Controllers;
using RPSLSGame.Models;
using RPSLSGame.Services;

namespace RPSLSGame.Tests.Controllers
{
    public class ChoiceControllerTests
    {
        private readonly Mock<IRandomChoiceService> _randomChoiceServiceMock;
        private readonly ChoiceController _choiceController;

        public ChoiceControllerTests()
        {
            _randomChoiceServiceMock = new Mock<IRandomChoiceService>(MockBehavior.Strict);
            _choiceController = new ChoiceController(_randomChoiceServiceMock.Object);
        }

        [Fact]
        public async Task GetRandomChoice_ReturnsOk_WithRandomChoice()
        {
            // Arrange
            var expectedChoice = new ChoiceModel(1, "rock");
            _randomChoiceServiceMock.Setup(service => service.GetRandomChoiceAsync())
                                    .ReturnsAsync(expectedChoice);

            // Act
            var result = await _choiceController.GetRandomChoice();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualChoice = Assert.IsType<ChoiceModel>(okResult.Value);
            Assert.Equal(expectedChoice, actualChoice);
        }

        [Fact]
        public async Task GetRandomChoice_Returns503_WhenHttpRequestExceptionThrown()
        {
            // Arrange
            _randomChoiceServiceMock.Setup(service => service.GetRandomChoiceAsync())
                                    .ThrowsAsync(new HttpRequestException());

            // Act
            var result = await _choiceController.GetRandomChoice();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(503, statusCodeResult.StatusCode);
            Assert.Equal("Failed to retrieve random choice. Please try again later.", statusCodeResult.Value);
        }
    }
}