namespace QuizApp.Server.Models.ViewModels
{
    public class QuizConverter
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int GamesPlayed { get; set; }
        public DateTime DateCreated { get; set; }
        public int MaxScore { get; set; }
        public List<QuestionView>? Questions { get; set; } = new List<QuestionView>();

        public static QuizView ConvertQuiz(Quiz quiz)
        {
            var quizView = new QuizView();

            quizView.Id = quiz.Id;
            quizView.Title = quiz.Title;
            quizView.GamesPlayed = quiz.GamesPlayed;
            quizView.DateCreated = quiz.DateCreated;
            quizView.MaxScore = quiz.MaxScore;

            var _questions = new List<QuestionView>();
            foreach (var q in quiz.Questions)
            {
                _questions.Add(QuestionConverter.ConvertQuestion(q));
            }
            quizView.Questions = _questions;

            return quizView;
        }
    }
}
