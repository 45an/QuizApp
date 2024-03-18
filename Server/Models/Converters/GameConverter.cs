namespace QuizApp.Server.Models.ViewModels
{
    public class GameConverter
    {
        public static GameView ConvertGame(Game game)
        {
            var gameView = new GameView();

            gameView.Id = game.Id;
            gameView.QuizId = game.QuizId;
            gameView.UserId = game.UserId;
            gameView.Score = game.Score;
            return gameView;
        }
    }
}
