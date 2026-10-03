using TicTacToe.Ai;
using TicTacToe.Config;
using TicTacToe.Game;
using Xunit;

namespace TicTacToe.Tests;

public sealed class MinimaxPlayerTests
{
    private static readonly MinimaxPlayer Ai = new(new GameConfig(3));

    [Fact]
    public void ChoosesWinningMoveInOne()
    {
        var board = new Board(3, Player.O);
        Play(board, 0, 3, 1, 4);

        Assert.Equal(Player.O, board.CurrentPlayer);
        var move = Ai.ChooseMove(board);
        Assert.Equal(2, move);
        board.TryMove(move);
        Assert.Equal(GameResult.OWins, board.Result);
    }

    [Fact]
    public void BlocksHumanWinningMove()
    {
        var board = new Board(3);
        Play(board, 0, 4, 1);

        Assert.Equal(Player.O, board.CurrentPlayer);
        var move = Ai.ChooseMove(board);
        Assert.Equal(2, move);
    }

    [Fact]
    public void PrefersOwnWinOverBlocking()
    {
        var board = new Board(3);
        Play(board, 0, 3, 1, 4, 8);

        Assert.Equal(Player.O, board.CurrentPlayer);
        var move = Ai.ChooseMove(board);
        Assert.Equal(5, move);
    }

    private static void Play(Board board, params int[] moves)
    {
        foreach (var move in moves)
        {
            Assert.True(board.TryMove(move));
        }
    }
}
