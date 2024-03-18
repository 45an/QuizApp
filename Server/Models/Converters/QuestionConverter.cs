namespace QuizApp.Server.Models.ViewModels
{
    public class QuestionConverter
    {
        public static QuestionView ConvertQuestion(Question question)
        {
            var questionView = new QuestionView();

            questionView.Id = question.Id;
            questionView.QuizId = question.QuizId;
            questionView.Questions = question.Questions;
            questionView.Answer = question.Answer;
            questionView.Media = question.Media;
            questionView.Time = question.Time;
            questionView.MultipleChoice = question.MultipleChoice;

            var _mocks = new List<MockView>();
            foreach (var m in question.MocksAnswer)
            {
                _mocks.Add(MockConverter.ConvertMock(m));
            }
            questionView.MocksAnswer = _mocks;

            return questionView;
        }
    }
}
