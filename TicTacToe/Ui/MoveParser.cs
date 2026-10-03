using TicTacToe.Game;

namespace TicTacToe.Ui;

public static class MoveParser
{
    public static bool TryParse(string? input, Board board, out int index, out string error)
    {
        index = -1;
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(input))
        {
            error = "Введите номер клетки или координаты: строка столбец.";
            return false;
        }

        var parts = input.Trim().Split([' ', ',', ';', '\t'], StringSplitOptions.RemoveEmptyEntries);

        try
        {
            if (parts.Length == 1)
            {
                if (!int.TryParse(parts[0], out var number))
                {
                    error = "Не удалось разобрать номер клетки.";
                    return false;
                }

                var max = board.Size * board.Size;
                if (number < 1 || number > max)
                {
                    error = $"Номер клетки должен быть от 1 до {max}.";
                    return false;
                }

                index = number - 1;
                return true;
            }

            if (parts.Length == 2
                && int.TryParse(parts[0], out var row)
                && int.TryParse(parts[1], out var col))
            {
                if (row < 1 || row > board.Size || col < 1 || col > board.Size)
                {
                    error = $"Координаты должны быть от 1 до {board.Size}.";
                    return false;
                }

                index = board.ToIndex(row - 1, col - 1);
                return true;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            error = "Клетка вне доски.";
            return false;
        }

        error = "Введите номер клетки (например, 5) или два числа: строка столбец (например, 2 2).";
        return false;
    }
}
