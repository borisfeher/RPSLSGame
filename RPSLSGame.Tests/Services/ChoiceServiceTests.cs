using RPSLSGame.Services;

namespace RPSLSGame.Tests.Services
{
    public class ChoiceServiceTests
    {
        private readonly ChoiceService _choiceService;

        public ChoiceServiceTests()
        {
            _choiceService = new ChoiceService();
        }

        [Fact]
        public void GetChoices_ReturnsFiveChoices()
        {
            // Act
            var result = _choiceService.GetChoices();

            // Assert
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void GetChoices_ReturnsCorrectChoices()
        {
            // Act
            var result = _choiceService.GetChoices().ToList();

            // Assert
            Assert.Equal(1, result[0].Id);
            Assert.Equal("rock", result[0].Name);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("paper", result[1].Name);

            Assert.Equal(3, result[2].Id);
            Assert.Equal("scissors", result[2].Name);

            Assert.Equal(4, result[3].Id);
            Assert.Equal("lizard", result[3].Name);

            Assert.Equal(5, result[4].Id);
            Assert.Equal("spock", result[4].Name);
        }
    }
}