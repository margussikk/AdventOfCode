using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2016.Assembunny;

namespace AdventOfCode.Year2016.Day25;

[Puzzle(2016, 25, "Clock Signal")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines.Select(Instruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        // Assume that everyone gets the same instructions but with different register values
        var value1 = ((CopyInstruction)_instructions[1]).Argument1.Value!.Value;
        var value2 = ((CopyInstruction)_instructions[2]).Argument1.Value!.Value;
        var thresholdNumber = value1 * value2;

        var a = 0;
        while (a < thresholdNumber || (a % 2 == 1)) // Find smallest even suitable number
        {
            if (a % 2 == 1)
            {
                a *= 2;
            }
            else
            {
                a = a * 2 + 1;
            }
        }

        var answer = a - thresholdNumber;

        return new PuzzleAnswer(answer, 158);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}