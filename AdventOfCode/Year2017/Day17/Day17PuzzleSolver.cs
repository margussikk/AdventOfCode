using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day17;

[Puzzle(2017, 17, "Spinlock")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private int _steps;

    public void ParseInput(string[] inputLines)
    {
        _steps = int.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var buffer = new List<int> { 0 };
        int currentPosition = 0;

        for (var value = 1; value <= 2017; value++)
        {
            currentPosition = ((currentPosition + _steps) % buffer.Count) + 1;
            if (currentPosition == buffer.Count)
            {
                buffer.Add(value);
            }
            else
            {
                buffer.Insert(currentPosition, value);
            }
        }

        var answer = buffer[(currentPosition + 1) % buffer.Count];

        return new PuzzleAnswer(answer, 204);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var bufferLength = 1;
        int currentPosition = 0;

        var answer = 0;

        for (var value = 1; value <= 50_000_000; value++)
        {
            currentPosition = ((currentPosition + _steps) % bufferLength) + 1;
            if (currentPosition == 1)
            {
                answer = value;
            }

            bufferLength++;
        }

        return new PuzzleAnswer(answer, 28954211);
    }
}