using RPSLSGame.Models;

namespace RPSLSGame.Services
{
    public class RandomChoiceService : IRandomChoiceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _randomNumberApiUrl;
        private static readonly string[] choices = { "rock", "paper", "scissors", "lizard", "spock" };

        public RandomChoiceService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _randomNumberApiUrl = configuration["RandomNumberApi:Url"] ?? throw new ArgumentNullException(nameof(configuration), "RandomNumberApi:Url is not configured.");
        }

        public async Task<ChoiceModel> GetRandomChoiceAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync(_randomNumberApiUrl);
            int randomNumber = int.Parse(response);

            int choiceIndex = randomNumber % choices.Length;
            string selectedChoice = choices[choiceIndex];

            return new ChoiceModel(choiceIndex + 1, selectedChoice);
        }
    }
}