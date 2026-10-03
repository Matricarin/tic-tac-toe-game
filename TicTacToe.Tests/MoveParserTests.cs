using TicTacToe.Game;
using TicTacToe.Ui;
using Xunit;

namespace TicTacToe.Tests;

public sealed class MoveParserTests
{
    [Theory]
    [InlineData("5", 4)]
    [InlineData("1 1", 0)]
    [InlineData("2,3", 5)]
    public void ParsesCellNumberAndCoordinates(string input, int expectedIndex)
    {
        var board = new Board(3);
        Assert.True(MoveParser.TryParse(input, board, out var index, out var error));
        Assert.Equal(expectedIndex, index);
        Assert.Equal(string.Empty, error);
    }

    [Fact]
    public void RejectsOutOfRange()
    {
        var board = new Board(3);
        Assert.False(MoveParser.TryParse("10", board, out _, out var error));
        Assert.Contains("1 до 9", error);
    }
}
