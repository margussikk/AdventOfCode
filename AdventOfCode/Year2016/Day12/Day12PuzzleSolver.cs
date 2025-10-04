using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

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
        var fibonacciN = ((SetRegisterInstruction)_instructions[2]).Value
            + (partTwo ? ((SetRegisterInstruction)_instructions[5]).Value
                       : 0);
        var fibonacci = MathFunctions.Fibonacci(fibonacciN + 2);

        var multiplicand = ((SetRegisterInstruction)_instructions[16]).Value;
        var multiplier = ((SetRegisterInstruction)_instructions[17]).Value;

        return fibonacci + multiplicand * multiplier;
    }
}
