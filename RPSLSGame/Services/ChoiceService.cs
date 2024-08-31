using RPSLSGame.Models;

namespace RPSLSGame.Services
{
    public class ChoiceService : IChoiceService
    {
        private readonly List<string> choices = new List<string>
        {
            "rock", "paper", "scissors", "lizard", "spock"
        };

        public IEnumerable<ChoiceModel> GetChoices()
        {
            return choices.Select((choice, index) => new ChoiceModel(index + 1, choice));
        }
    }
}