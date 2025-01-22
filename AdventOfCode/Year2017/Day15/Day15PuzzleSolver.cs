using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day15;

[Puzzle(2017, 15, "Dueling Generators")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private long _generatorAStartValue;
    private long _generatorBStartValue;

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            if (line.StartsWith("Generator A starts with"))
            {
                _generatorAStartValue = long.Parse(line["Generator A starts with".Length..]);
            }
            else if (line.StartsWith("Generator B starts with"))
            {
                _generatorBStartValue = long.Parse(line["Generator B starts with".Length..]);
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GenerateA(_generatorAStartValue)
            .Zip(GenerateB(_generatorBStartValue))
            .Take(40_000_000)
            .Count(x => (x.First & 0xFFFF) == (x.Second & 0xFFFF));

        return new PuzzleAnswer(answer, 631);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GenerateA(_generatorAStartValue).Where(x => (x & 0x03) == 0)
            .Zip(GenerateB(_generatorBStartValue).Where(x => (x & 0x07) == 0))
            .Take(5_000_000)
            .Count(x => (x.First & 0xFFFF) == (x.Second & 0xFFFF));

        return new PuzzleAnswer(answer, 279);
    }

    private static IEnumerable<long> GenerateA(long value)
    {
        while (true)
        {
            value = (value * 16807) % 2147483647L;
            yield return value;
        }
    }

    private static IEnumerable<long> GenerateB(long value)
    {
        while (true)
        {
            value = (value * 48271) % 2147483647L;
            yield return value;
        }
    }
}