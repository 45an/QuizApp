namespace QuizApp.Server.Models.ViewModels
{
    public class MediaView
    {
        public Guid Guid { get; set; }
        public string? path { get; set; }   
        public byte[]? FileBytes { get; set; }

    }
}
