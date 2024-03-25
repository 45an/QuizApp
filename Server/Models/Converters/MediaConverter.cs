namespace QuizApp.Server.Models.ViewModels
{
    public class MediaConverter
    {
        public static MediaView ConvertMedia(Media media)
        {
            var mediaView = new MediaView();

            mediaView.Guid = media.Guid;
            mediaView.Hash = media.Hash;
            mediaView.Path = media.Path;
            mediaView.ContentType = media.ContentType;
            mediaView.FileBytes = media.FileBytes;

            return mediaView;
        }
    }
}
