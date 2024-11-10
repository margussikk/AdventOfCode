using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2021.Day18;

[Puzzle(2021, 18, "Snailfish")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<SnailfishNumber> _snailfishNumbers = [];

    public void ParseInput(string[] inputLines)
    {
        _snailfishNumbers = inputLines.Select(SnailfishNumber.Parse)
                                      .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var finalSum = _snailfishNumbers[0].DeepCopy();

        for (var i = 1; i < _snailfishNumbers.Count; i++)
        {
            finalSum = finalSum.AddAndReduce(_snailfishNumbers[i].DeepCopy());
        }

        var answer = finalSum.Magnitude();

        return new PuzzleAnswer(answer, 4124);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = int.MinValue;

        for (var i = 0; i < _snailfishNumbers.Count; i++)
        {
            for (var j = 0; i != j && j < _snailfishNumbers.Count; j++)
            {
                var sum = _snailfishNumbers[i]
                    .DeepCopy()
                    .AddAndReduce(_snailfishNumbers[j].DeepCopy());

                answer = Math.Max(answer, sum.Magnitude());
            }
        }

        return new PuzzleAnswer(answer, 4673);
    }
}