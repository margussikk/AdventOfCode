using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2021.Day04;

internal class Board
{
    public Grid<BoardNumber> BoardNumbers { get; } = new(5, 5);

    public void MarkNumber(int value)
    {
        var boardNumber = BoardNumbers.Cast<GridCell<BoardNumber>?>().FirstOrDefault(cell => cell.HasValue && cell.Value.Object.Value == value);
        if (boardNumber != null)
        {
            boardNumber.Value.Object.Marked = true;
        }
    }

    public void Reset()
    {
        foreach (var boardNumber in BoardNumbers)
        {
            boardNumber.Object.Marked = false;
        }
    }

    public bool IsWinner()
    {
        for (var row = 0; row < 5; row++)
        {
            if (BoardNumbers.Row(row).All(x => x.Object.Marked))
            {
                return true;
            }
        }

        for (var column = 0; column < 5; column++)
        {
            if (BoardNumbers.Column(column).All(x => x.Object.Marked))
            {
                return true;
            }
        }

        return false;
    }

    public int SumOfUnmarkedNumbers()
    {
        return BoardNumbers.Where(boardNumber => !boardNumber.Object.Marked)
                           .Sum(boardNumber => boardNumber.Object.Value);
    }

    public static Board Parse(string[] lines)
    {
        var board = new Board();

        for (var row = 0; row < 5; row++)
        {
            var boardRowNumbers = lines[row].SplitToNumbers<int>(' ');

            for (var column = 0; column < boardRowNumbers.Length; column++)
            {
                board.BoardNumbers[row, column] = new BoardNumber(boardRowNumbers[column]);
            }
        }

        return board;
    }
}
