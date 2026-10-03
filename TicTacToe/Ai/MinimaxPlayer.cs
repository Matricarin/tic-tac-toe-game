using TicTacToe.Config;
using TicTacToe.Game;

namespace TicTacToe.Ai;

public sealed class MinimaxPlayer
{
    public const Player Computer = Player.O;
    public const Player Human = Player.X;

    private readonly int _maxDepth;

    public MinimaxPlayer(GameConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);
        _maxDepth = config.AiSearchDepth;
    }

    public int ChooseMove(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);

        if (board.Result.IsFinished())
        {
            throw new InvalidOperationException("Игра уже закончена.");
        }

        if (board.CurrentPlayer != Computer)
        {
            throw new InvalidOperationException("Сейчас ход человека, а не компьютера.");
        }

        var bestScore = int.MinValue;
        var bestIndex = -1;

        foreach (var index in board.EmptyCells())
        {
            board.TryMove(index);
            var score = Minimax(board, _maxDepth - 1, int.MinValue, int.MaxValue);
            board.Undo();

            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = index;
            }
        }

        if (bestIndex < 0)
        {
            throw new InvalidOperationException("Нет доступных ходов.");
        }

        return bestIndex;
    }

    private int Minimax(Board board, int depth, int alpha, int beta)
    {
        if (board.Result.IsFinished() || depth == 0)
        {
            return Evaluate(board);
        }

        if (board.CurrentPlayer == Computer)
        {
            var best = int.MinValue;
            foreach (var index in board.EmptyCells())
            {
                board.TryMove(index);
                best = Math.Max(best, Minimax(board, depth - 1, alpha, beta));
                board.Undo();
                alpha = Math.Max(alpha, best);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return best;
        }
        else
        {
            var best = int.MaxValue;
            foreach (var index in board.EmptyCells())
            {
                board.TryMove(index);
                best = Math.Min(best, Minimax(board, depth - 1, alpha, beta));
                board.Undo();
                beta = Math.Min(beta, best);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return best;
        }
    }

    private static int Evaluate(Board board)
    {
        return board.Result switch
        {
            GameResult.OWins => 10_000 - board.MoveCount,
            GameResult.XWins => -10_000 + board.MoveCount,
            GameResult.Draw => 0,
            _ => Heuristic(board)
        };
    }

    private static int Heuristic(Board board)
    {
        var score = 0;
        score += ScoreLines(board, dRow: 0, dCol: 1);
        score += ScoreLines(board, dRow: 1, dCol: 0);
        score += ScoreOneLine(board, 0, 0, 1, 1);
        score += ScoreOneLine(board, 0, board.Size - 1, 1, -1);
        return score;
    }

    private static int ScoreLines(Board board, int dRow, int dCol)
    {
        var score = 0;
        var count = board.Size;
        for (var i = 0; i < count; i++)
        {
            var startRow = dRow == 0 ? i : 0;
            var startCol = dCol == 0 ? i : 0;
            score += ScoreOneLine(board, startRow, startCol, dRow, dCol);
        }

        return score;
    }

    private static int ScoreOneLine(Board board, int startRow, int startCol, int dRow, int dCol)
    {
        var x = 0;
        var o = 0;
        for (var i = 0; i < board.Size; i++)
        {
            var cell = board[startRow + i * dRow, startCol + i * dCol];
            if (cell == Player.X)
            {
                x++;
            }
            else if (cell == Player.O)
            {
                o++;
            }
        }

        if (x > 0 && o > 0)
        {
            return 0;
        }

        if (o > 0)
        {
            return (int)Math.Pow(10, o);
        }

        if (x > 0)
        {
            return -(int)Math.Pow(10, x);
        }

        return 0;
    }
}
