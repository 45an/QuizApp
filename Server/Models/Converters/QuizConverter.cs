namespace QuizApp.Server.Models.ViewModels
{
    public class QuizConverter
    {
        public static QuizView Convert(Quiz quiz)
        {
            var quizView = new QuizView
            {
                Id = quiz.Id,
                Title = quiz.Title,
                GamesPlayed = quiz.GamesPlayed,
                DateCreated = quiz.DateCreated,
                MaxScore = quiz.MaxScore
            };

            var _questions = new List<QuestionView>();
            foreach (var q in quiz.Questions)
            {
                _questions.Add(QuestionConverter.Convert(q));
            }
            quizView.Questions = _questions;

            return quizView;
        }
    }
}
