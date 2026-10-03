namespace TicTacToe.Game;

public enum Player
{
    None = 0,
    X = 1,
    O = 2
}

public static class PlayerExtensions
{
    public static Player Opponent(this Player player) => player switch
    {
        Player.X => Player.O,
        Player.O => Player.X,
        _ => Player.None
    };

    public static char ToMark(this Player player) => player switch
    {
        Player.X => 'X',
        Player.O => 'O',
        _ => ' '
    };
}
