using Microsoft.AspNetCore.Mvc;
using Moq;
using RPSLSGame.Controllers;
using RPSLSGame.Models;
using RPSLSGame.Services;

namespace RPSLSGame.Tests.Controllers
{
    public class ChoicesControllerTests
    {
        private readonly Mock<IChoiceService> _IChoiceServiceMock;
        private readonly ChoicesController _choicesController;

        public ChoicesControllerTests()
        {
            _IChoiceServiceMock = new Mock<IChoiceService>();
            _choicesController = new ChoicesController(_IChoiceServiceMock.Object);
        }

        [Fact]
        public void GetChoices_ReturnsOk_WithListOfChoices()
        {
            // Arrange
            var expectedChoices = new List<ChoiceModel>
            {
                new ChoiceModel(1, "rock"),
                new ChoiceModel(2, "paper"),
                new ChoiceModel(3, "scissors"),
                new ChoiceModel(4, "lizard"),
                new ChoiceModel(5, "spock")
            };

            _IChoiceServiceMock.Setup(service => service.GetChoices())
                              .Returns(expectedChoices);

            // Act
            var result = _choicesController.GetChoices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualChoices = Assert.IsType<List<ChoiceModel>>(okResult.Value);
            Assert.Equal(expectedChoices.Count, actualChoices.Count);
            Assert.True(expectedChoices.SequenceEqual(actualChoices, new ChoiceModelComparer()));
        }
    }

    public class ChoiceModelComparer : IEqualityComparer<ChoiceModel>
    {
        public bool Equals(ChoiceModel? x, ChoiceModel? y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(ChoiceModel obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return HashCode.Combine(obj.Id, obj.Name);
        }
    }
}