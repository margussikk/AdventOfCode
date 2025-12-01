using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2025.Day01;

[Puzzle(2025, 1, "Secret Entrance")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private const int StartingPoint = 50;
    private const int TotalPoints = 100;
    private IReadOnlyList<Turn> _turns = [];

    public void ParseInput(string[] inputLines)
    {
        _turns = [.. inputLines.Select(Turn.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;
        var pointsAt = StartingPoint;

        foreach (var turn in _turns)
        {
            var shift = turn.Direction == GridDirection.Left ? -turn.Clicks : turn.Clicks;

            pointsAt = MathFunctions.Modulo(pointsAt + shift, TotalPoints);
            if (pointsAt == 0)
            {
                answer++;
            }
        }

        return new PuzzleAnswer(answer, 1081);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;
        var pointsAt = StartingPoint;

        foreach (var turn in _turns)
        {
            answer += Math.DivRem(turn.Clicks, TotalPoints, out var remainder);

            var shift = turn.Direction == GridDirection.Left ? -remainder : remainder;
            var newPointsAt = pointsAt + shift;

            if (pointsAt is not 0 && !(newPointsAt is > 0 and < 100))
            {
                answer++;
            }

            pointsAt = MathFunctions.Modulo(newPointsAt, TotalPoints);
        }

        return new PuzzleAnswer(answer, 6689);
    }
}