using RPSLSGame.Models;

namespace RPSLSGame.Services
{
    public interface IChoiceService
    {
        IEnumerable<ChoiceModel> GetChoices();
    }
}