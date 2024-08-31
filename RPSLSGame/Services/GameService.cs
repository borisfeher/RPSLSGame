using System.Text.Json;

namespace RPSLSGame.Services
{
    public class GameService : IGameService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _randomNumberApiUrl;
        private static readonly string[] choices = { "rock", "paper", "scissors", "lizard", "spock" };
        private static readonly Dictionary<string, string[]> winningMoves = new Dictionary<string, string[]>
    {
        { "rock", new[] { "scissors", "lizard" } },
        { "paper", new[] { "rock", "spock" } },
        { "scissors", new[] { "paper", "lizard" } },
        { "lizard", new[] { "spock", "paper" } },
        { "spock", new[] { "scissors", "rock" } }
    };

        public GameService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _randomNumberApiUrl = configuration["RandomNumberApi:Url"] ?? throw new ArgumentNullException(nameof(configuration), "RandomNumberApi:Url is not configured.");
        }

        public async Task<int> GetComputerChoiceAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync(_randomNumberApiUrl);

            var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, int>>(response);

            if (jsonResponse == null || !jsonResponse.ContainsKey("random_number"))
            {
                throw new InvalidOperationException("Invalid response received from the random number API.");
            }

            int randomNumber = jsonResponse["random_number"];
            return randomNumber % choices.Length + 1;
        }

        public string GetChoiceName(int choiceId)
        {
            if (choiceId < 1 || choiceId > choices.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(choiceId), "Choice ID must be between 1 and 5.");
            }

            return choices[choiceId - 1];
        }

        public string DetermineWinner(string player, string computer)
        {
            if (player == computer)
            {
                return "tie";
            }

            return winningMoves[player].Contains(computer) ? "win" : "lose";
        }
    }
}