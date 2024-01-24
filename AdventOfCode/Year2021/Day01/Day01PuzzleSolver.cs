using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day01;

[Puzzle(2021, 1, "Sonar Sweep")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private List<int> _depthMeasurements = [];

    public void ParseInput(string[] inputLines)
    {
        _depthMeasurements = inputLines.Select(int.Parse)
                                       .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(1);

        return new PuzzleAnswer(answer, 1553);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(3);

        return new PuzzleAnswer(answer, 1597);
    }

    private int GetAnswer(int windowSize)
    {
        var count = 0;
        var previousSum = int.MaxValue;

        for (var i = 0; i <= _depthMeasurements.Count - windowSize; i++)
        {
            var sum = _depthMeasurements.GetRange(i, windowSize).Sum();
            if (sum > previousSum)
            {
                count++;
            }

            previousSum = sum;
        }

        return count;
    }
}