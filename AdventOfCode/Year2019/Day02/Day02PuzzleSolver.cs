using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day02;

[Puzzle(2019, 2, "1202 Program Alarm")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();

    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
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
            for(var verb = 0; verb <= 99; verb++)
            {
                var memoryValue = GetAnswer(noun, verb);
                if (memoryValue == 19690720)
                {
                    var answer = 100 * noun + verb;
                    return new PuzzleAnswer(answer, 3146);
                }
            }
        }

        return new PuzzleAnswer("ERROR", "NOT ERROR");
    }

    public long GetAnswer(int address1Value, int address2Value)
    {
        var computer = new IntCodeComputer();
        computer.Load(_program);

        computer.WriteMemory(1, address1Value);
        computer.WriteMemory(2, address2Value);

        computer.Run();

        return computer.ReadMemory(0);
    }
}