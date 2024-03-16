using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Text;
using System.Text;

namespace AdventOfCode.Year2022.Day10;

[Puzzle(2022, 10, "Cathode-Ray Tube")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;
        var cycle = 1;
        var x = 1;

        foreach (var instruction in _instructions)
        {
            for (var i = 0; i < instruction.CyclesCount; i++)
            {
                if ((cycle - 20) % 40 == 0)
                {
                    var signalStrength = cycle * x;
                    answer += signalStrength;
                }

                cycle++;
            }

            if (instruction is AddXInstruction addXInstruction)
            {
                x += addXInstruction.Parameter;
            }
        }

        return new PuzzleAnswer(answer, 12740);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var stringBuilder = new StringBuilder();

        var cycle = 1;
        var x = 1;

        foreach (var instruction in _instructions)
        {
            for (var i = 0; i < instruction.CyclesCount; i++)
            {
                var position = (cycle - 1) % 40;

                if (Math.Abs(position - x) <= 1)
                {
                    stringBuilder.Append('#');
                }
                else
                {
                    stringBuilder.Append('.');
                }

                if (cycle % 40 == 0)
                {
                    stringBuilder.AppendLine();
                }

                cycle++;
            }

            if (instruction is AddXInstruction addXInstruction)
            {
                x += addXInstruction.Parameter;
            }
        }

        var answer = Ocr.Parse(stringBuilder.ToString());

        return new PuzzleAnswer(answer, "RBPARAGF");
    }
}