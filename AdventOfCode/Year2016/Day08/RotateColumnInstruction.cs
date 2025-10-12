using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day08;

internal class RotateColumnInstruction : Instruction
{
    public int Column { get; }
    public int Shift { get; }

    public RotateColumnInstruction(int column, int shift)
    {
        Column = column;
        Shift = shift;
    }

    public override void Execute(Grid<bool> grid)
    {
        var newColumn = new bool[grid.Height];

        for (var row = 0; row < grid.Height; row++)
        {
            newColumn[(row + Shift) % grid.Height] = grid[row, Column];
        }

        for (var row = 0; row < grid.Height; row++)
        {
            grid[row, Column] = newColumn[row];
        }
    }
}
