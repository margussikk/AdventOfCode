using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day14;

[Puzzle(2015, 14, "Reindeer Olympics")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Reindeer> _reindeers = [];

    public void ParseInput(string[] inputLines)
    {
        _reindeers = [.. inputLines.Select(Reindeer.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _reindeers.Max(r => r.CalculateDistanceAfterSeconds(2503));

        return new PuzzleAnswer(answer, 2655);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var points = new int[_reindeers.Count];

        for (var seconds = 1; seconds <= 2503; seconds++)
        {
            var distances = _reindeers
                .Select(r => r.CalculateDistanceAfterSeconds(seconds))
                .ToList();

            var maxDistance = distances.Max();

            for (var i = 0; i < distances.Count; i++)
            {
                if (distances[i] == maxDistance)
                {
                    points[i]++;
                }
            }
        }

        var answer = points.Max();

        return new PuzzleAnswer(answer, 1059);
    }
}