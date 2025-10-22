using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2015.Day03;

[Puzzle(2015, 3, "Perfectly Spherical Houses in a Vacuum")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<GridDirection> _directions = [];

    public void ParseInput(string[] inputLines)
    {
        _directions = [.. inputLines[0].Select(CharExtensions.ParseArrowToGridDirection)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _directions.Accumulate(GridCoordinate.Zero, (accumulation, direction) => accumulation.Move(direction))
                                .Distinct()
                                .Count() + 1;

        return new PuzzleAnswer(answer, 2081);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var coordinates = new GridCoordinate[]
        {
            GridCoordinate.Zero,
            GridCoordinate.Zero
        };

        var visited = coordinates.ToHashSet();

        for (var index = 0; index < _directions.Count; index++)
        {
            var santaIndex = index % 2;
            coordinates[santaIndex] = coordinates[santaIndex].Move(_directions[index]);
            visited.Add(coordinates[santaIndex]);
        }

        var answer = visited.Count;

        return new PuzzleAnswer(answer, 2341);
    }
}