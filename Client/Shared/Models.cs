using Microsoft.AspNetCore.Components.Forms;

namespace QuizApp.Client.Shared.Models
{
    class GameModel
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public QuizModel? QuizModel { get; set; }
        public string? UserId { get; set; }
        public int Score { get; set; }
    }

    class QuizModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public virtual MediaModel? Media { get; set; }
        public IBrowserFile File { get; set; }
        public DateTime DateCreated { get; set; }
        public int MaxScore { get; set; }
        public int GamesPlayed { get; set; }
        public string? UserId { get; set; }
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }

    class MediaModel
    {
        public Guid? MediaGuid { get; set; }
        public string? Hash { get; set; }
        public string? Path { get; set; }
        public string? ContentType { get; set; }
        public byte[]? FileBytes { get; set; }
    }

    class QuestionModel
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int Index { get; set; }
        public string Questions { get; set; }
        public string Answer { get; set; }
        public virtual MediaModel? Media { get; set; }
        public IBrowserFile File { get; set; }
        public int Time { get; set; }

        public bool MultipleChoice { get; set; }
        public List<MockModel> MocksAnswers { get; set; } = new List<MockModel>();
    }

    class MockModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string? MockAnswer { get; set; }
    }
}
