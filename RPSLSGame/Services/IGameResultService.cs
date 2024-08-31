using RPSLSGame.Models;

namespace RPSLSGame.Services
{
    public interface IGameResultService
    {
        IEnumerable<GameResult> GetRecentResults(int count = 10);
        void AddResult(GameResult result);
        void ResetResults();
    }
}