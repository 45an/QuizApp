namespace QuizApp.Server.Models.ViewModels
{
    public class QuizView
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Title { get; set; }
        public MediaView? Media { get; set; }
        public DateTime DateCreated { get; set; }
        public int MaxScore { get; set; }
        public int GamesPlayed { get; set; }
        public List<QuestionView>? Questions { get; set; } = new List<QuestionView>();
    }
}
