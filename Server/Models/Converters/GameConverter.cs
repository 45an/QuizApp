namespace QuizApp.Server.Models.ViewModels
{
    public class GameConverter
    {
        public static GameView Convert(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game), "Game cannot be null.");
            }

            var gameView = new GameView
            {
                Id = game.Id,
                QuizId = game.QuizId,
                UserId = game.UserId,
                Score = game.Score
            };

            return gameView;
        }
    }
}
