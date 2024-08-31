using RPSLSGame.Models;

namespace RPSLSGame.Services
{
    public class GameResultService : IGameResultService
    {
        private readonly List<GameResult> _results = new List<GameResult>();

        public IEnumerable<GameResult> GetRecentResults(int count = 10)
        {
            return _results.OrderByDescending(r => r.TimeStamp).Take(count);
        }

        public void AddResult(GameResult result)
        {
            _results.Add(result);
        }

        public void ResetResults()
        {
            _results.Clear();
        }
    }
}