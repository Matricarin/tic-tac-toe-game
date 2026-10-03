using TicTacToe.Config;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests;

public sealed class BoardTests
{
    [Fact]
    public void NewBoard_IsEmptyAndInProgress()
    {
        var board = new Board(3);

        Assert.Equal(GameResult.InProgress, board.Result);
        Assert.Equal(Player.X, board.CurrentPlayer);
        Assert.Equal(9, board.EmptyCells().Count());
        Assert.Equal(Player.None, board[1, 1]);
    }

    [Fact]
    public void TryMove_PlacesMarkAndSwitchesPlayer()
    {
        var board = new Board(3);

        Assert.True(board.TryMove(0, 0));
        Assert.Equal(Player.X, board[0, 0]);
        Assert.Equal(Player.O, board.CurrentPlayer);
    }

    [Fact]
    public void TryMove_RejectsOccupiedCell()
    {
        var board = new Board(3);
        Assert.True(board.TryMove(1, 1));
        Assert.False(board.TryMove(1, 1));
        Assert.Equal(Player.O, board.CurrentPlayer);
    }

    [Theory]
    [InlineData(0, 1, 2)]
    public void XWins_ByRow(int a, int b, int c)
    {
        var board = new Board(3);
        Play(board, a, 3, b, 4, c);
        Assert.Equal(GameResult.XWins, board.Result);
    }

    [Fact]
    public void OWins_ByColumn()
    {
        var board = new Board(3);
        Play(board, 1, 0, 2, 3, 4, 6);
        Assert.Equal(GameResult.OWins, board.Result);
    }

    [Fact]
    public void XWins_ByMainDiagonal()
    {
        var board = new Board(3);
        Play(board, 0, 1, 4, 2, 8);
        Assert.Equal(GameResult.XWins, board.Result);
    }

    [Fact]
    public void OWins_ByAntiDiagonal()
    {
        var board = new Board(3);
        Play(board, 0, 2, 1, 4, 3, 6);
        Assert.Equal(GameResult.OWins, board.Result);
    }

    [Fact]
    public void Draw_WhenBoardIsFullWithoutWinner()
    {
        var board = new Board(3);
        Play(board, 0, 1, 2, 4, 3, 6, 5, 8, 7);
        Assert.Equal(GameResult.Draw, board.Result);
        Assert.False(board.TryMove(0));
    }

    [Fact]
    public void FiveByFive_WinsByFullRow()
    {
        var board = new Board(5);
        for (var col = 0; col < 4; col++)
        {
            Assert.True(board.TryMove(0, col));
            Assert.True(board.TryMove(1, col));
        }

        Assert.True(board.TryMove(0, 4));
        Assert.Equal(GameResult.XWins, board.Result);
    }

    [Fact]
    public void GameConfig_RejectsUnknownSize()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameConfig(4));
    }

    [Fact]
    public void Undo_RestoresPreviousState()
    {
        var board = new Board(3);
        board.TryMove(0);
        board.Undo();
        Assert.Equal(Player.None, board.CellAt(0));
        Assert.Equal(Player.X, board.CurrentPlayer);
        Assert.Equal(0, board.MoveCount);
    }

    private static void Play(Board board, params int[] moves)
    {
        foreach (var move in moves)
        {
            Assert.True(board.TryMove(move), $"Не удалось походить в {move}");
        }
    }
}
