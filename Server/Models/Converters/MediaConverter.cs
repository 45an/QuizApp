namespace QuizApp.Server.Models.ViewModels
{
    public class MediaConverter
    {
        public static MediaView Convert(Media media)
        {
            var mediaView = new MediaView
            {
                Guid = media.Guid,
                Hash = media.Hash,
                Path = media.Path,
                ContentType = media.ContentType,
                FileBytes = media.FileBytes
            };

            return mediaView;
        }
    }
}
