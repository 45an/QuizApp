namespace QuizApp.Server.Models.ViewModels
{
    public class QuestionView
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string? Questions { get; set; }
        public string? Answer { get; set; }
        public bool CorrectAnswer { get; set; }
        public MediaView? Media { get; set; }
        public int Time { get; set; }
        public bool MultipleChoice { get; set; }
        public List<MockView>? MocksAnswer { get; set; } = new List<MockView>();
    }
}
