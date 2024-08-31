namespace RPSLSGame.Models
{
    public class GamePlayResponse
    {
        public string Results { get; set; }
        public ChoiceData Player { get; set; }
        public ChoiceData Computer { get; set; }
    }
}