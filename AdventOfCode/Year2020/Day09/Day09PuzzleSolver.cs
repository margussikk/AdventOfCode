using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2020.Day09;

[Puzzle(2020, 9, "Encoding Error")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private List<long> _numbers = [];

    public void ParseInput(string[] inputLines)
    {
        _numbers = inputLines.Select(long.Parse)
                             .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = FindInvalidNumber();

        return new PuzzleAnswer(answer, 27911108L);
    }


    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0L;

        var invalidNumber = FindInvalidNumber();

        var runningSum = _numbers[0] + _numbers[1];

        var indexRange = new NumberRange<int>(0, 1);
        while (indexRange.End < _numbers.Count)
        {
            if (runningSum < invalidNumber)
            {
                runningSum += _numbers[indexRange.End + 1];
                indexRange = new NumberRange<int>(indexRange.Start, indexRange.End + 1);
            }
            else if (runningSum > invalidNumber)
            {
                runningSum -= _numbers[indexRange.Start];
                indexRange = new NumberRange<int>(indexRange.Start + 1, indexRange.End);
            }
            else
            {
                var range = _numbers.Skip(indexRange.Start).Take(indexRange.Length);

                answer = range.Min() + range.Max();
                break;
            }
        }

        return new PuzzleAnswer(answer, 4023754L);
    }

    public long FindInvalidNumber()
    {
        var preambleLength = 25;

        for (var index = preambleLength; index < _numbers.Count; index++)
        {
            var isValid = false;

            for (var index1 = index - preambleLength; !isValid && index1 < index - 1; index1++)
            {
                for (var index2 = index1 + 1; !isValid && index2 < index; index2++)
                {
                    if (_numbers[index1] != _numbers[index2] && _numbers[index1] + _numbers[index2] == _numbers[index])
                    {
                        isValid = true;
                    }
                }
            }

            if (!isValid)
            {
                return _numbers[index];
            }
        }

        return 0L;
    }
}