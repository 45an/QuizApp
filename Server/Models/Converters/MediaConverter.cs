namespace QuizApp.Server.Models.ViewModels
{
    public class MediaConverter
    {
        public static MediaView Convert(Media media)
        {
            if (media == null)
            {
                throw new ArgumentNullException(nameof(media), "Media cannot be null.");
            }

            var mediaView = new MediaView
            {
                Guid = media.MediaGuid,
                Hash = media.Hash,
                Path = media.Path,
                ContentType = media.ContentType,
                FileBytes = media.FileBytes
            };

            return mediaView;
        }
    }
}
