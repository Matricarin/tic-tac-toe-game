namespace TicTacToe.Game;

public sealed class Board
{
    private readonly Player[] _cells;
    private readonly Stack<(int Index, Player PreviousCurrent)> _history = new();

    public Board(int size, Player startingPlayer = Player.X)
    {
        if (size < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Размер доски должен быть не меньше 3.");
        }

        if (startingPlayer is not (Player.X or Player.O))
        {
            throw new ArgumentException("Первым должен ходить X или O.", nameof(startingPlayer));
        }

        Size = size;
        _cells = new Player[size * size];
        CurrentPlayer = startingPlayer;
        Result = GameResult.InProgress;
    }

    public int Size { get; }
    public Player CurrentPlayer { get; private set; }
    public GameResult Result { get; private set; }
    public int MoveCount => _history.Count;

    public Player this[int row, int col] => _cells[ToIndex(row, col)];

    public Player CellAt(int index) => _cells[index];

    public IEnumerable<int> EmptyCells()
    {
        for (var i = 0; i < _cells.Length; i++)
        {
            if (_cells[i] == Player.None)
            {
                yield return i;
            }
        }
    }

    public bool TryMove(int row, int col) => TryMove(ToIndex(row, col));

    public bool TryMove(int index)
    {
        if (Result.IsFinished() || index < 0 || index >= _cells.Length || _cells[index] != Player.None)
        {
            return false;
        }

        var mover = CurrentPlayer;
        _history.Push((index, mover));
        _cells[index] = mover;
        Result = EvaluateAfterMove(index, mover);
        CurrentPlayer = Result.IsFinished() ? mover : mover.Opponent();
        return true;
    }

    public void Undo()
    {
        if (_history.Count == 0)
        {
            throw new InvalidOperationException("Нет хода для отмены.");
        }

        var (index, previousCurrent) = _history.Pop();
        _cells[index] = Player.None;
        CurrentPlayer = previousCurrent;
        Result = GameResult.InProgress;
    }

    public int ToIndex(int row, int col)
    {
        if (row < 0 || row >= Size || col < 0 || col >= Size)
        {
            throw new ArgumentOutOfRangeException($"Клетка ({row},{col}) вне доски {Size}x{Size}.");
        }

        return row * Size + col;
    }

    public (int Row, int Col) FromIndex(int index)
    {
        if (index < 0 || index >= _cells.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Индекс клетки вне доски.");
        }

        return (index / Size, index % Size);
    }

    private GameResult EvaluateAfterMove(int index, Player mover)
    {
        var row = index / Size;
        var col = index % Size;

        if (IsLine(row, 0, 0, 1, mover)
            || IsLine(0, col, 1, 0, mover)
            || (row == col && IsLine(0, 0, 1, 1, mover))
            || (row + col == Size - 1 && IsLine(0, Size - 1, 1, -1, mover)))
        {
            return mover == Player.X ? GameResult.XWins : GameResult.OWins;
        }

        return _history.Count == _cells.Length ? GameResult.Draw : GameResult.InProgress;
    }

    private bool IsLine(int startRow, int startCol, int dRow, int dCol, Player player)
    {
        for (var i = 0; i < Size; i++)
        {
            if (_cells[(startRow + i * dRow) * Size + (startCol + i * dCol)] != player)
            {
                return false;
            }
        }

        return true;
    }
}
