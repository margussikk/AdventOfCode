using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2017.Day11;

[Puzzle(2017, 11, "Hex Ed")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private List<GridDirection> _directions = [];

    public void ParseInput(string[] inputLines)
    {
        _directions = inputLines[0].Split(',')
                                   .Select(direction => direction switch
                                   {
                                       "nw" => GridDirection.UpLeft,
                                       "n" => GridDirection.Up,
                                       "ne" => GridDirection.UpRight,
                                       "sw" => GridDirection.DownLeft,
                                       "s" => GridDirection.Down,
                                       "se" => GridDirection.DownRight,
                                       _ => throw new InvalidOperationException($"Invalid direction: {direction}")
                                   })
                                   .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        FollowDirections(out var answer, out _);

        return new PuzzleAnswer(answer, 808);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        FollowDirections(out _, out var answer);

        return new PuzzleAnswer(answer, 1556);
    }

    private void FollowDirections(out int finalDistance, out int furthestDistance)
    {
        furthestDistance = 0;

        var coordinate = new FlatTopHexCoordinate(0, 0);
        foreach (var direction in _directions)
        {
            coordinate = coordinate.Move(direction);
            furthestDistance = int.Max(furthestDistance, CountSteps(coordinate));
        }

        finalDistance = CountSteps(coordinate);
    }


    private static int CountSteps(FlatTopHexCoordinate coordinate)
    {
        return new int[] { int.Abs(coordinate.Q), int.Abs(coordinate.R), int.Abs(coordinate.S) }.Max();
    }
}