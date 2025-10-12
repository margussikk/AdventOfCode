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
        _directions = [.. inputLines[0].Select(c => c.ParseArrowToGridDirection())];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var coordinate = GridCoordinate.Zero;

        var visited = new HashSet<GridCoordinate>
        {
            coordinate
        };

        foreach (var direction in _directions)
        {
            coordinate = coordinate.Move(direction);
            visited.Add(coordinate);
        }

        var answer = visited.Count;

        return new PuzzleAnswer(answer, 2081);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var santaCount = 2;

        var coordinates = Enumerable.Range(0, santaCount).Select(x => GridCoordinate.Zero).ToArray();

        var visited = coordinates.ToHashSet();

        foreach (var directionChunk in _directions.Chunk(santaCount))
        {
            for (var i = 0; i < santaCount; i++)
            {
                coordinates[i] = coordinates[i].Move(directionChunk[i]);
                visited.Add(coordinates[i]);
            }
        }

        var answer = visited.Count;

        return new PuzzleAnswer(answer, 2341);
    }
}