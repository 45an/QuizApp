namespace QuizApp.Server.Models.ViewModels
{
    public class GameView
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string? UserId { get; set; }
        public int Score { get; set; }
    }
}
