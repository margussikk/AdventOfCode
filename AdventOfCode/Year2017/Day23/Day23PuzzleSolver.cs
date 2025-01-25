using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2017.Day23;

[Puzzle(2017, 23, "Coprocessor Conflagration")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var coprocessor = new Coprocessor(_instructions);

        coprocessor.Run(-1);

        return new PuzzleAnswer(coprocessor.MulInvokes, 3969);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var subInstructions = _instructions.Where(i => i.Code == InstructionCode.Sub)
                                           .ToList();

        var breakpoint = _instructions.IndexOf(subInstructions[1]) + 1;

        var coprocessor = new Coprocessor(_instructions);
        coprocessor.Registers[0] = 1;
        coprocessor.Run(breakpoint);

        var firstNumber = coprocessor.Registers[subInstructions[0].OperandA.Register];
        var lastNumber = coprocessor.Registers[subInstructions[1].OperandA.Register];
        var step = -subInstructions[^1].OperandB.Value;

        var answer = 0;

        for (var value = firstNumber; value <= lastNumber; value += step)
        {
            if (!MathFunctions.IsPrime(value))
            {
                answer++;
            }
        }

        return new PuzzleAnswer(answer, 917);
    }
}