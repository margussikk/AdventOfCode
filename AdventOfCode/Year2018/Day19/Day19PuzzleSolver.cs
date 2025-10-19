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
        var breakAt = _instructions.FindIndex(i => i.OpCode == OpCode.SetI && i.A == 0 && i.C == _instructionPointerBinding);

        var answer = GetAnswer(0, breakAt);

        return new PuzzleAnswer(answer, 1092);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var breakAt = _instructions.Count - 1;

        var answer = GetAnswer(1, breakAt);

        return new PuzzleAnswer(answer, 13406472);
    }

    private long GetAnswer(long register0, int breakAt)
    {
        var device = new Device([register0, 0, 0, 0, 0, 0]);
        device.RunProgram(_instructions, _instructionPointerBinding, breakAt);

        // Assume the program always has GtRR instruction and the B operand refers to the number register
        var gtrrInstruction = _instructions.First(x => x.OpCode == OpCode.GtRR);
        var number = device.Registers[(int)gtrrInstruction.B];

        return MathFunctions.Divisors(number).Sum();
    }
}