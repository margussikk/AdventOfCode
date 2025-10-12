using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day01;

internal class Instruction
{
    public GridDirection TurnDirection { get; set; }

    public int BlockSteps { get; set; }

    public static Instruction Parse(string input)
    {
        return new Instruction
        {
            TurnDirection = input[0].ParseLetterToGridDirection(),
            BlockSteps = int.Parse(input[1..])
        };
    }
}
