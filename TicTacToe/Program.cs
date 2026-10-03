using TicTacToe.Ai;
using TicTacToe.Config;
using TicTacToe.Game;
using TicTacToe.Ui;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var renderer = new ConsoleRenderer();
renderer.Write("Крестики-нолики: вы играете X, компьютер — O.");
renderer.Write("Выигрывает полная линия: ряд, столбец или главная диагональ.");

while (true)
{
    var config = AskBoardSize(renderer);
    var starting = Random.Shared.Next(2) == 0 ? Player.X : Player.O;
    var board = new Board(config.BoardSize, starting);
    var ai = new MinimaxPlayer(config);

    renderer.Write(starting == Player.X
        ? "Жребий: первым ходите вы (X)."
        : "Жребий: первым ходит компьютер (O).");

    PlayRound(board, ai, renderer);

    renderer.Write("Сыграть ещё? (д/н)");
    var again = Console.ReadLine()?.Trim() ?? string.Empty;
    if (!again.StartsWith("д", StringComparison.OrdinalIgnoreCase)
        && !again.StartsWith("y", StringComparison.OrdinalIgnoreCase))
    {
        renderer.Write("До встречи.");
        break;
    }
}

static GameConfig AskBoardSize(ConsoleRenderer renderer)
{
    while (true)
    {
        renderer.Write($"Размер доски ({string.Join(", ", GameConfig.AllowedBoardSizes)}):");
        var input = Console.ReadLine();
        if (int.TryParse(input, out var size) && GameConfig.IsAllowedSize(size))
        {
            return new GameConfig(size);
        }

        renderer.Write("Нужно ввести 3, 5 или 10.");
    }
}

static void PlayRound(Board board, MinimaxPlayer ai, ConsoleRenderer renderer)
{
    renderer.Write("Ход: номер клетки или «строка столбец» (нумерация с 1).");

    while (!board.Result.IsFinished())
    {
        renderer.RenderBoard(board);

        if (board.CurrentPlayer == Player.X)
        {
            renderer.Write("Ваш ход:");
            var input = Console.ReadLine();
            if (!MoveParser.TryParse(input, board, out var index, out var error))
            {
                renderer.Write(error);
                continue;
            }

            if (!board.TryMove(index))
            {
                renderer.Write("Клетка занята или недоступна, попробуйте снова.");
            }

            continue;
        }

        renderer.Write("Ход компьютера...");
        var move = ai.ChooseMove(board);
        board.TryMove(move);
        var (row, col) = board.FromIndex(move);
        renderer.Write($"Компьютер поставил O в клетку {move + 1} (строка {row + 1}, столбец {col + 1}).");
    }

    renderer.RenderBoard(board);
    renderer.RenderResult(board.Result);
}
