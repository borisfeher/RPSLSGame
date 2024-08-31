using Moq;
using RPSLSGame.Services;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace RPSLSGame.Tests.Services
{
    public class RandomChoiceServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly RandomChoiceService _randomChoiceService;

        public RandomChoiceServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(config => config["RandomNumberApi:Url"])
                              .Returns("https://codechallenge.boohma.com/random");

            _randomChoiceService = new RandomChoiceService(_httpClientFactoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task GetRandomChoiceAsync_ReturnsExpectedChoice()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("1")
            };

            var mockHttpMessageHandler = new MockHttpMessageHandler(mockResponse);

            var httpClient = new HttpClient(mockHttpMessageHandler);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _randomChoiceService.GetRandomChoiceAsync();

            // Assert
            Assert.Equal(2, result.Id); // Since "1" % 5 is 1 and choices[1] is "paper"
            Assert.Equal("paper", result.Name);
        }
    }
}