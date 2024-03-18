namespace QuizApp.Server.Models.ViewModels
{
    public class MediaConverter
    {
        public static MediaView ConvertMedia(Media media)
        {
            var mediaView = new MediaView();

            mediaView.Guid = media.Guid;
            mediaView.Path = media.Path;
            mediaView.FileBytes = media.FileBytes;
            mediaView.ContentType = media.ContentType;

            return mediaView;
        }
    }
}
