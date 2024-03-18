namespace QuizApp.Server.Models.ViewModels
{
    public class MockConverter
    {
        public static MockView ConvertMock(Mock mock)
        {
            var mockView = new MockView();

            mockView.Id = mock.Id;
            mockView.QuestionId = mock.QuestionId;
            mockView.MockAnswer = mock.MockAnswer;

            return mockView;
        }
    }
}
