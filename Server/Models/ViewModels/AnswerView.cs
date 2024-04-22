using QuizApp.Server.Models.ViewModels;

namespace QuizApp.Server.Models
{
    public class AnswerView
    {
        public int Id { get; set; }
        public QuizView? OriginalQuiz { get; set; }
        public QuizView? AnswerQuiz { get; set; }
        public string? UserId { get; set; }
    }
}
