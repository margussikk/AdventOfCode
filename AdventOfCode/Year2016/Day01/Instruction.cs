using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day01;
internal class Instruction
{
    public GridDirection TurnDirection { get; set; }

    public int Blocks { get; set; }

    public static Instruction Parse(string input)
    {
        return new Instruction
        {
            TurnDirection = input[0] == 'L' ? GridDirection.Left : GridDirection.Right,
            Blocks = int.Parse(input[1..])
        };
    }
}
