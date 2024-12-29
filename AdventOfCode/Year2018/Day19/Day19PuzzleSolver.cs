using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Year2018.Common;

namespace AdventOfCode.Year2018.Day19;

[Puzzle(2018, 19, "Go With The Flow")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private int _instructionPointerBinding = 0;
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        if (!inputLines[0].StartsWith("#ip"))
        {
            throw new InvalidOperationException("First line in input wasn't #ip");
        }

        _instructionPointerBinding = int.Parse(inputLines[0]["#ip ".Length..]);
        _instructions = inputLines.Skip(1).Select(Instruction.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var device = new Device([0, 0, 0, 0, 0, 0]);

        device.RunProgram(_instructions, _instructionPointerBinding);

        var answer = device.Registers[0];

        return new PuzzleAnswer(answer, 1092);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var device = new Device([1, 0, 0, 0, 0, 0]);

        var instructions = _instructions.SkipLast(1) // Let it run only until large number is generated
                                        .ToList();

        device.RunProgram(instructions, _instructionPointerBinding);

        // Assume the program always has GtRR instruction and the B operand refers to the large number register
        var gtrrInstruction = instructions.First(x => x.OpCode == OpCode.GtRR);
        var largeNumber = device.Registers[gtrrInstruction.B];

        var answer = MathFunctions.GetDivisors(largeNumber).Sum();

        return new PuzzleAnswer(answer, 13406472);
    }
}