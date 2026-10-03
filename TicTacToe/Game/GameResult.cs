namespace TicTacToe.Game;

public enum GameResult
{
    InProgress,
    XWins,
    OWins,
    Draw
}

public static class GameResultExtensions
{
    public static bool IsFinished(this GameResult result) => result != GameResult.InProgress;
}
