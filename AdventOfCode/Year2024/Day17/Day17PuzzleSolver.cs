using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2024.Day17;

[Puzzle(2024, 17, "Chronospatial Computer")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private long _registerA;
    private long _registerB;
    private long _registerC;

    private List<int> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _registerA = long.Parse(inputLines[0]["Register A: ".Length..]);
        _registerB = long.Parse(inputLines[1]["Register B: ".Length..]);
        _registerC = long.Parse(inputLines[2]["Register C: ".Length..]);

        _program = inputLines[4]["Program: ".Length..].Split(',')
                                                      .Select(int.Parse)
                                                      .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computer = new Computer(_registerA, _registerB, _registerC, _program);

        var output = computer.Run();

        var answer = string.Join(',', output);

        return new PuzzleAnswer(answer, "4,3,2,6,4,5,3,2,4");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0L;

        for (var i = 0; i < _program.Count; i++)
        {
            answer <<= 3; // Assume it's always Adv 3

            for (var value = 0; value < int.MaxValue; value++)
            {
                var testA = answer + value;

                var computer = new Computer(testA, _registerB, _registerC, _program);

                var output = computer.Run();
                if (output.SequenceEqual(_program.TakeLast(output.Count)))
                {
                    answer = testA;
                    break;
                }
            }
        }

        return new PuzzleAnswer(answer, 164540892147389L);
    }
}