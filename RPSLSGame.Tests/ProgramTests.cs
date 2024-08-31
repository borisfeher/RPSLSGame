using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RPSLSGame.Services;

namespace RPSLSGame.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void TestServiceRegistrations()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddHttpClient();

            // Add services as they are in Program.cs
            builder.Services.AddSingleton<ChoiceService>();
            builder.Services.AddSingleton<GameResultService>();
            builder.Services.AddTransient<GameService>();
            builder.Services.AddTransient<RandomChoiceService>();
            builder.Services.AddScoped<IRandomChoiceService, RandomChoiceService>();

            var serviceProvider = builder.Services.BuildServiceProvider();

            // Act
            var choiceService = serviceProvider.GetService<ChoiceService>();
            var gameResultService = serviceProvider.GetService<GameResultService>();
            var gameService = serviceProvider.GetService<GameService>();
            var randomChoiceService = serviceProvider.GetService<IRandomChoiceService>();

            // Assert
            Assert.NotNull(choiceService);
            Assert.NotNull(gameResultService);
            Assert.NotNull(gameService);
            Assert.NotNull(randomChoiceService);
        }
    }
}