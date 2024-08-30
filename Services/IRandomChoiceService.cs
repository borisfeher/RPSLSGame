using RPSLSGame.Models;

namespace RPSLSGame.Services
{
    public interface IRandomChoiceService
    {
        Task<ChoiceModel> GetRandomChoiceAsync();
    }
}