using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Year2016.Assembunny;

namespace AdventOfCode.Year2016.Day12;

[Puzzle(2016, 12, "Leonardo's Monorail")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines.Select(Instruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(false);
        return new PuzzleAnswer(answer, 318077);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(true);
        return new PuzzleAnswer(answer, 9227731);
    }

    private int GetAnswer(bool partTwo)
    {
        // Using hardcoded instruction indexes.
        // Assuming that everyone gets the same instructions just with different register values.
        var fibonacciN = ((CopyInstruction)_instructions[2]).Argument1.Value!.Value
            + (partTwo ? ((CopyInstruction)_instructions[5]).Argument1.Value!.Value
                       : 0);
        var fibonacci = MathFunctions.Fibonacci(fibonacciN + 2);

        var multiplicand = ((CopyInstruction)_instructions[16]).Argument1.Value!.Value;
        var multiplier = ((CopyInstruction)_instructions[17]).Argument1.Value!.Value;

        return fibonacci + multiplicand * multiplier;
    }
}
