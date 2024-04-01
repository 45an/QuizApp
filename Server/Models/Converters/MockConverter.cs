namespace QuizApp.Server.Models.ViewModels
{
    public class MockConverter
    {
        public static MockView Convert(Mock mock)
        {
            var mockView = new MockView
            {
                Id = mock.Id,
                QuestionId = mock.QuestionId,
                MockAnswer = mock.MockAnswer
            };

            return mockView;
        }
    }
}
