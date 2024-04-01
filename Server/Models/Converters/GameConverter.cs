namespace QuizApp.Server.Models.ViewModels
{
    public class GameConverter
    {
        public static GameView Convert(Game game)
        {
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
