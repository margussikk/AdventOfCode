using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day08;

internal class RotateRowInstruction : Instruction
{
    public int Row { get; }
    public int Shift { get; }

    public RotateRowInstruction(int row, int shift)
    {
        Row = row;
        Shift = shift;
    }

    public override void Execute(Grid<bool> grid)
    {
        var newRow = new bool[grid.Width];

        for (var column = 0; column < grid.Width; column++)
        {
            newRow[(column + Shift) % grid.Width] = grid[Row, column];
        }

        for (var column = 0; column < grid.Width; column++)
        {
            grid[Row, column] = newRow[column];
        }
    }
}
