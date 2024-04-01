namespace QuizApp.Server.Models.ViewModels
{
    public class QuestionConverter
    {
        public static QuestionView Convert(Question question)
        {
            var questionView = new QuestionView
            {
                Id = question.Id,
                QuizId = question.QuizId,
                Questions = question.Questions,
                Answer = question.Answer,
                Media = MediaConverter.Convert(question.Media),
                Time = question.Time,
                MultipleChoice = question.MultipleChoice
            };

            var _mocks = new List<MockView>();
            foreach (var m in question.MocksAnswers)
            {
                _mocks.Add(MockConverter.Convert(m));
            }
            questionView.MocksAnswer = _mocks;

            return questionView;
        }
    }
}
