namespace QuizApp.Server.Models.ViewModels.Requests
{
    public class SaveGameRequest
    {
        public Answer Answer { get; set; }
        public int Score { get; set; }
    }
}
