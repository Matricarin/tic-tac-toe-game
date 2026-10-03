using System.Text;
using TicTacToe.Game;

namespace TicTacToe.Ui;

public sealed class ConsoleRenderer
{
    private readonly TextWriter _output;

    public ConsoleRenderer(TextWriter? output = null)
    {
        _output = output ?? Console.Out;
    }

    public void Write(string message) => _output.WriteLine(message);

    public void RenderBoard(Board board)
    {
        var sb = new StringBuilder();
        var size = board.Size;
        var cellWidth = size * size >= 100 ? 4 : 3;

        sb.Append("   ");
        for (var col = 0; col < size; col++)
        {
            sb.Append((col + 1).ToString().PadLeft(cellWidth));
        }

        sb.AppendLine();

        for (var row = 0; row < size; row++)
        {
            sb.Append((row + 1).ToString().PadLeft(2)).Append(' ');
            for (var col = 0; col < size; col++)
            {
                var player = board[row, col];
                var text = player == Player.None
                    ? (board.ToIndex(row, col) + 1).ToString()
                    : player.ToMark().ToString();
                sb.Append(text.PadLeft(cellWidth));
            }

            sb.AppendLine();
        }

        _output.WriteLine(sb.ToString());
    }

    public void RenderResult(GameResult result)
    {
        var message = result switch
        {
            GameResult.XWins => "Вы победили (X)!",
            GameResult.OWins => "Компьютер победил (O).",
            GameResult.Draw => "Ничья.",
            _ => "Игра продолжается."
        };
        _output.WriteLine(message);
    }
}
