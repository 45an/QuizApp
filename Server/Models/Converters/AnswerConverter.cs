namespace QuizApp.Server.Models.ViewModels
{
    public class AnswerConverter
    {
        public static AnswerView Convert(Answer answer)
        {
            if (answer == null)
            {
                throw new ArgumentNullException(nameof(answer), "Question cannot be null.");
            }

            var answerView = new AnswerView
            {
                Id = answer.Id,
                OriginalQuiz = QuizConverter.Convert(answer.OriginalQuiz),
                AnswerQuiz = QuizConverter.Convert(answer.AnswerQuiz),
                UserId = answer.UserId
            };

            return answerView;
        }
    }
}
