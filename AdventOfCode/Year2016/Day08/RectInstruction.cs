using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day08;

internal class RectInstruction : Instruction
{
    public int Width { get; }
    public int Height { get; }

    public RectInstruction(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public override void Execute(Grid<bool> grid)
    {
        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                grid[row, column] = true;
            }
        }
    }
}
