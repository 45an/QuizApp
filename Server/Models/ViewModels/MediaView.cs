namespace QuizApp.Server.Models.ViewModels
{
    public class MediaView
    {
        public Guid Guid { get; set; }
        public string? Hash { get; set; }
        public string? Path { get; set; }
        public string? ContentType { get; set; }
        public byte[]? FileBytes { get; set; }
    }
}
