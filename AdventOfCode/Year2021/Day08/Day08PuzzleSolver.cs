using AdventOfCode.Framework.Puzzle;
using System.Numerics;

namespace AdventOfCode.Year2021.Day08;

[Puzzle(2021, 8, "Seven Segment Search")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private List<Entry> _entries = [];

    public void ParseInput(string[] inputLines)
    {
        _entries = inputLines.Select(Entry.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _entries.Sum(entry =>
        {
            var digitSignals = GetDigitSignals(entry);
            var digitSignals1478 = new[]
            {
                digitSignals[1], digitSignals[4], digitSignals[7], digitSignals[8]
            };

            return entry.Outputs.Count(digitSignals1478.Contains);
        });

        return new PuzzleAnswer(answer, 301);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _entries.Sum(entry =>
        {
            var digitSignals = GetDigitSignals(entry);
            return entry.Outputs.Aggregate(0, (acc, next) => acc * 10 + Array.IndexOf(digitSignals, next));
        });

        return new PuzzleAnswer(answer, 908067);
    }

    private static int[] GetDigitSignals(Entry entry)
    {
        var signalsBySegmentCount = entry.Signals.ToLookup(x => BitOperations.PopCount((uint)x));

        var digits = new int[10];
        digits[1] = signalsBySegmentCount[2].First();
        digits[4] = signalsBySegmentCount[4].First();
        digits[7] = signalsBySegmentCount[3].First();
        digits[8] = signalsBySegmentCount[7].First();

        foreach (var signal in signalsBySegmentCount[5])
        {
            if ((signal & digits[1]) == digits[1])
            {
                digits[3] = signal;
            }
            else
            {
                var notFour = 0b111_1111 & ~digits[4];
                if ((signal & notFour) == notFour)
                {
                    digits[2] = signal;
                }
                else
                {
                    digits[5] = signal;
                }
            }
        }

        foreach (var signal in signalsBySegmentCount[6])
        {
            if ((signal & digits[4]) == digits[4])
            {
                digits[9] = signal;
            }
            else if ((signal & digits[1]) == digits[1])
            {
                digits[0] = signal;
            }
            else
            {
                digits[6] = signal;
            }
        }

        return digits;
    }
}