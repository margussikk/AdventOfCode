using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;
using AdventOfCode.Year2016.Assembunny;

namespace AdventOfCode.Year2016.Day23;

[Puzzle(2016, 23, "Safe Cracking")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines.Select(Instruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(7);

        return new PuzzleAnswer(answer, 13050);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(12);

        return new PuzzleAnswer(answer, 479009610L);
    }

    private long GetAnswer(int a)
    {
        var factorial = MathFunctions.Factorial(a);

        // Assume that everyone gets the same instructions but with different register values
        var value1 = ((CopyInstruction)_instructions[19]).Argument1.Value!.Value;
        var value2 = ((JumpInstruction)_instructions[20]).Argument1.Value!.Value;

        return factorial + value1 * value2;
    }
}