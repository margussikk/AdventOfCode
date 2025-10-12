using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day08;

internal abstract class Instruction
{
    public abstract void Execute(Grid<bool> grid);

    public static Instruction Parse(string line)
    {
        var parts = line.Split(' ');
        return parts[0] switch
        {
            "rect" => new RectInstruction(int.Parse(parts[1].Split('x')[0]), int.Parse(parts[1].Split('x')[1])),
            "rotate" when parts[1] == "row" => new RotateRowInstruction(int.Parse(parts[2].Split('=')[1]), int.Parse(parts[4])),
            "rotate" when parts[1] == "column" => new RotateColumnInstruction(int.Parse(parts[2].Split('=')[1]), int.Parse(parts[4])),
            _ => throw new InvalidOperationException("Invalid instruction")
        };
    }
}
