using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day02;

[Puzzle(2019, 2, "1202 Program Alarm")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SplitToNumbers<long>(',');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(12, 2);

        return new PuzzleAnswer(answer, 8017076);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        for (var noun = 0; noun <= 99; noun++)
        {
            for (var verb = 0; verb <= 99; verb++)
            {
                var memoryValue = GetAnswer(noun, verb);
                if (memoryValue != 19690720) continue;

                var answer = 100 * noun + verb;
                return new PuzzleAnswer(answer, 3146);
            }
        }

        return new PuzzleAnswer("ERROR", "NOT ERROR");
    }

    private long GetAnswer(int address1Value, int address2Value)
    {
        var computer = new IntCodeComputer(_program)
        {
            Memory =
            {
                [1] = address1Value,
                [2] = address2Value
            }
        };

        computer.Run();

        return computer.Memory[0];
    }
}