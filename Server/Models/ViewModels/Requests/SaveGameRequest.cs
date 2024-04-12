namespace QuizApp.Server.Models.ViewModels.Requests
{
    public class SaveGameRequest
    {
        public int QuizId { get; set; }
        public int Score { get; set; }
    }
}
