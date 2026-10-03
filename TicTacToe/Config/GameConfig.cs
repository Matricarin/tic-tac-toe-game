namespace TicTacToe.Config;

public sealed class GameConfig
{
    public static readonly int[] AllowedBoardSizes = [3, 5, 10];

    public int BoardSize { get; }

    public GameConfig(int boardSize)
    {
        if (!IsAllowedSize(boardSize))
        {
            throw new ArgumentOutOfRangeException(
                nameof(boardSize),
                boardSize,
                $"Размер доски должен быть одним из: {string.Join(", ", AllowedBoardSizes)}.");
        }

        BoardSize = boardSize;
    }

    public static bool IsAllowedSize(int boardSize) => AllowedBoardSizes.Contains(boardSize);

    public int CellCount => BoardSize * BoardSize;

    public int AiSearchDepth => BoardSize switch
    {
        3 => 9,
        5 => 4,
        _ => 2
    };
}
