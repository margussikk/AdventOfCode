using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2017.Day21;
internal static class GridUtils
{
    public static int GetGridBitmask(GridBase<bool> grid)
    {
        var bitmask = 0;

        foreach (var cell in grid)
        {
            bitmask <<= 1;

            if (cell.Object)
            {
                bitmask++;
            }
        }

        bitmask += (grid.Width << 16);

        return bitmask;
    }
}
