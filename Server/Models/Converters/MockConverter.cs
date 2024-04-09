namespace QuizApp.Server.Models.ViewModels
{
    public class MockConverter
    {
        public static MockView Convert(Mock mock)
        {
            if (mock == null)
            {
                throw new ArgumentNullException(nameof(mock), "Mock cannot be null.");
            }

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
