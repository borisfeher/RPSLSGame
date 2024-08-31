namespace RPSLSGame.Models
{
    public class GameResult
    {
        public string? PlayerChoice { get; set; }
        public string? ComputerChoice { get; set; }
        public string? Result { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}