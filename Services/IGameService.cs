namespace RPSLSGame.Services
{
    public interface IGameService
    {
        Task<int> GetComputerChoiceAsync();
        string GetChoiceName(int choiceId);
        string DetermineWinner(string player, string computer);
    }
}