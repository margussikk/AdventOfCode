using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2021.Day04;

internal class Board
{
    public Grid<BoardNumber> BoardNumbers { get; } = new Grid<BoardNumber>(5, 5);

    public void MarkNumber(int value)
    {
        foreach (var boardNumber in BoardNumbers)
        {
            if (boardNumber.Object.Value == value)
            {
                boardNumber.Object.Marked = true;
                return;
            }
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
        var sum = 0;

        foreach (var boardNumber in BoardNumbers)
        {
            if (!boardNumber.Object.Marked)
            {
                sum += boardNumber.Object.Value;
            }
        }

        return sum;
    }

    public static Board Parse(string[] lines)
    {
        var board = new Board();

        for (var row = 0; row < 5; row++)
        {
            var boardRowNumbers = lines[row]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            for (var column = 0; column < boardRowNumbers.Length; column++)
            {
                board.BoardNumbers[row, column] = new BoardNumber(boardRowNumbers[column]);
            }
        }

        return board;
    }
}
