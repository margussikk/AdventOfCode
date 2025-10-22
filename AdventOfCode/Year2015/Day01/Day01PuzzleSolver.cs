using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day01;

[Puzzle(2015, 1, "Not Quite Lisp")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Direction> _directions = [];

    public void ParseInput(string[] inputLines)
    {
        _directions = [.. inputLines[0]
            .Select(c => c switch
            {
                '(' => Direction.Up,
                ')' => Direction.Down,
                _ => throw new InvalidOperationException($"Invalid direction: {c}")
            })];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _directions.Sum(d => d == Direction.Up ? 1 : -1);

        return new PuzzleAnswer(answer, 138);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;
        var floor = 0;

        for (var position = 0; position < _directions.Count; position++)
        {
            var movement = _directions[position] == Direction.Up ? 1 : -1;

            floor += movement;
            if (floor < 0)
            {
                answer = position + 1;
                break;
            }
        }

        return new PuzzleAnswer(answer, 1771);
    }
}