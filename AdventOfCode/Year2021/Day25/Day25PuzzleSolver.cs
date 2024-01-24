using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2021.Day25;

[Puzzle(2021, 25, "Sea Cucumber")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private Grid<GridDirection> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(character => character switch
        {
            '.' => GridDirection.None,
            '>' => GridDirection.Right,
            'v' => GridDirection.Down,
            _ => throw new InvalidOperationException()
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        bool moved;

        var answer = 0;
        do
        {
            answer++;

            moved = MoveRight();
            moved = MoveDown() || moved;
        }
        while (moved);

        return new PuzzleAnswer(answer, 504);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }

    private bool MoveRight()
    {
        var updatedDirections = new List<(GridCoordinate coordinate, GridDirection direction)>();

        foreach (var gridCell in _grid)
        {
            if (gridCell.Object == GridDirection.Right)
            {
                var nextColumn = (gridCell.Coordinate.Column + 1) % _grid.Width;
                var nextCoordinate = new GridCoordinate(gridCell.Coordinate.Row, nextColumn);

                if (_grid[nextCoordinate] == GridDirection.None)
                {
                    updatedDirections.Add((gridCell.Coordinate, GridDirection.None));
                    updatedDirections.Add((nextCoordinate, GridDirection.Right));
                }
            }
        }

        foreach (var (coordinate, direction) in updatedDirections)
        {
            _grid[coordinate] = direction;
        }

        return updatedDirections.Count != 0;
    }

    private bool MoveDown()
    {
        var updatedDirections = new List<(GridCoordinate coordinate, GridDirection direction)>();
        
        foreach (var gridCell in _grid)
        {
            if (gridCell.Object == GridDirection.Down)
            {
                var nextRow = (gridCell.Coordinate.Row + 1) % _grid.Height;
                var nextCoordinate = new GridCoordinate(nextRow, gridCell.Coordinate.Column);

                if (_grid[nextCoordinate] == GridDirection.None)
                {
                    updatedDirections.Add((gridCell.Coordinate, GridDirection.None));
                    updatedDirections.Add((nextCoordinate, GridDirection.Down));
                }
            }
        }

        foreach (var (coordinate, direction) in updatedDirections)
        {
            _grid[coordinate] = direction;
        }

        return updatedDirections.Count != 0;
    }

    private static void Print(Grid<GridDirection> grid)
    {
        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                switch (grid[row, column])
                {
                    case GridDirection.Right:
                        Console.Write(">");
                        break;
                    case GridDirection.Down:
                        Console.Write("v");
                        break;
                    default:
                        Console.Write(".");
                        break;
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}