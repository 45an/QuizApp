namespace QuizApp.Server.Models.ViewModels
{
    public class UserView
    {
        public string? UserName { get; set; }
        public List<QuizView>? Quizzes { get; set; }

    }
}
