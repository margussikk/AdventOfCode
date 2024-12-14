using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2024.Day14;

[Puzzle(2024, 14, "Restroom Redoubt")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private List<Robot> _robots = [];

    public void ParseInput(string[] inputLines)
    {
        _robots = inputLines.Select(Robot.Parse)
                            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        const int seconds = 100;
        const int width = 101;
        const int height = 103;

        var robotsInQuadrants = new long[4];

        var betweenX = width / 2;
        var betweenY = height / 2;

        foreach (var robot in _robots)
        {
            var x = MathFunctions.Modulo(robot.Position.X + seconds * robot.Velocity.DX, width);
            var y = MathFunctions.Modulo(robot.Position.Y + seconds * robot.Velocity.DY, height);

            if (x == betweenX || y == betweenY)
            {
                continue;
            }
            
            var quadrant = x > betweenX ? 1 : 0;
            quadrant += y > betweenY ? 2 : 0;

            robotsInQuadrants[quadrant]++;
        }

        var answer = robotsInQuadrants.Aggregate(1L, (agg, curr) => agg *  curr);

        return new PuzzleAnswer(answer, 225648864L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var robots = _robots
            .Select(r => r.Clone())
            .ToList();

        var answer = 0;

        for (int seconds = 1; seconds < int.MaxValue; seconds++)
        {
            foreach (var robot in robots)
            {
                robot.Move();
            }

            if (robots.Select(r => r.Position)
                      .Distinct()
                      .Count() == robots.Count)
            {
                // Tracking the lowest safety factor also works
                answer = seconds;
                break;
            }
        }

        return new PuzzleAnswer(answer, 7847);
    }
}