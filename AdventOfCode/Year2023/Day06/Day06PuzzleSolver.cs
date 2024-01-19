using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2023.Day06;

[Puzzle(2023, 6, "Wait For It")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private List<Race> _part1Races = [];
    private Race _part2Race = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        var times = inputLines[0]["Time:".Length..].SelectToLongs(' ');

        var distances = inputLines[1]["Distance:".Length..].SelectToLongs(' ');

        _part1Races = times.Zip(distances)
                           .Select(z => new Race(z.First, z.Second))
                           .ToList();

        var time = inputLines[0]["Time:".Length..].SelectToOneLong();
        var distance = inputLines[1]["Distance:".Length..].SelectToOneLong();
        _part2Race = new Race(time, distance);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _part1Races.Aggregate(1L, (acc, current) => acc * current.CountWaysToBeatTheRecord());

        return new PuzzleAnswer(answer, 588588);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _part2Race.CountWaysToBeatTheRecord();

        return new PuzzleAnswer(answer, 34655848);
    }
}
