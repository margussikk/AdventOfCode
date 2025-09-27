using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace AdventOfCode.Year2016.Day01;

[Puzzle(2016, 1, "No Time for a Taxicab")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = Array.Empty<Instruction>();

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines[0].Split(',', StringSplitOptions.TrimEntries).Select(Instruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var gridWalker = new GridWalker(GridCoordinate.Zero, GridCoordinate.Zero, GridDirection.Up, 0);

        foreach (var instruction in _instructions)
        {
            gridWalker.Turn(instruction.TurnDirection);
            gridWalker.Step(instruction.Blocks);
        }

        var answer = gridWalker.Coordinate.ManhattanDistanceTo(gridWalker.StartCoordinate);

        return new PuzzleAnswer(answer, 253);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var gridWalker = new GridWalker(GridCoordinate.Zero, GridCoordinate.Zero, GridDirection.Up, 0);

        var visited = new HashSet<GridCoordinate> { gridWalker.Coordinate };

        foreach (var instruction in _instructions)
        {
            var alreadyVisited = false;

            gridWalker.Turn(instruction.TurnDirection);

            for (var i = 0; i < instruction.Blocks; i++)
            {
                gridWalker.Step();

                if (!visited.Add(gridWalker.Coordinate))
                {
                    alreadyVisited = true;
                    break;
                }
            }

            if (alreadyVisited)
            {
                break;
            }
        }

        var answer = gridWalker.Coordinate.ManhattanDistanceTo(gridWalker.StartCoordinate);

        return new PuzzleAnswer(answer, 126);
    }
}