namespace QuizApp.Server.Models.ViewModels
{
    public class QuizView
    {
      public int Id { get; set; }   
      public string? Title { get; set; }
      public int GamesPlayed { get; set; }  
      public int HighScore { get; set; }
      public List<QuestionView>? Questions { get; set; } = new List<QuestionView>();
    }
}
