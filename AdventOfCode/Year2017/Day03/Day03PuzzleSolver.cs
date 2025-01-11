using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2017.Day03;

[Puzzle(2017, 3, "Spiral Memory")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private readonly static GridDirection[] _spiralDirections = [GridDirection.Right, GridDirection.Up, GridDirection.Left, GridDirection.Down];
    private int _input;

    public void ParseInput(string[] inputLines)
    {
        _input = int.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetStepsFromSquare(_input);

        return new PuzzleAnswer(answer, 552);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetFirstLargerValue(_input);

        return new PuzzleAnswer(answer, 330785);
    }

    private static int GetStepsFromSquare(int square)
    {
        var stepsLeft = square - 1; // Squares start at 1

        var coordinate = GridCoordinate.Zero;

        // Blocks: R,U,L,L,D,D = 1*R + 1*U + 2*L + 2*D = 1*L + 1*D
        var blockIndex = 0;
        while (stepsLeft > 0)
        {
            var groupSize = 8 * blockIndex + 6;
            if (stepsLeft < groupSize)
            {
                break;
            }

            coordinate = coordinate.Move(GridDirection.DownLeft);

            stepsLeft -= groupSize;
            blockIndex++;
        }

        if (stepsLeft > 0)
        {
            // Leftover steps
            foreach (var direction in _spiralDirections)
            {
                var steps = int.Min(stepsLeft, GetMaxSteps(blockIndex, direction));
                coordinate = coordinate.Move(direction, steps);
                stepsLeft -= steps;

                if (stepsLeft == 0)
                {
                    break;
                }
            }
        }

        return coordinate.ManhattanDistanceTo(GridCoordinate.Zero);
    }


    private static int GetFirstLargerValue(int input)
    {
        var grid = new InfiniteGrid<int?>();

        var coordinate = GridCoordinate.Zero;
        grid[coordinate] = 1;

        var blockIndex = 0;
        while (true)
        {
            foreach (var direction in _spiralDirections)
            {
                var maxSteps = GetMaxSteps(blockIndex, direction);
                for (var step = 0; step < maxSteps; step++)
                {
                    coordinate = coordinate.Move(direction);
                    var value = grid.AroundNeighbors(coordinate).Where(c => c.Object != null).Sum(c => c.Object!.Value);
                    if (value > input)
                    {
                        return value;
                    }

                    grid[coordinate] = value;
                }
            }

            blockIndex++;
        }
    }

    private static int GetMaxSteps(int blockIndex, GridDirection direction)
    {
        return direction switch
        {
            GridDirection.Right or GridDirection.Up => 2 * blockIndex + 1,
            GridDirection.Left or GridDirection.Down => 2 * (blockIndex + 1),
            _ => throw new InvalidOperationException($"Unexpected direction: {direction}")
        };
    }
}