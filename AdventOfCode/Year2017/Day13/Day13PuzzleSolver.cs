using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day13;

[Puzzle(2017, 13, "Packet Scanners")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private List<Scanner> _scanners = [];

    public void ParseInput(string[] inputLines)
    {
        _scanners = inputLines.Select(line =>
                    {
                        var numbers = line.SplitToNumbers<int>(':', ' ');
                        return new Scanner(numbers[0], numbers[1]);
                    })
                    .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _scanners.Where(s => s.GetsCaught(0))
                              .Sum(s => s.Severity);

        return new PuzzleAnswer(answer, 1612);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        while (_scanners.Any(s => s.GetsCaught(answer)))
        {
            answer++;
        }

        return new PuzzleAnswer(answer, 3907994);
    }
}