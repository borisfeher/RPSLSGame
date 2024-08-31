namespace RPSLSGame.Models
{
    public class ChoiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ChoiceModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}