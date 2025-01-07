using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2022.Day23;

internal class Elf
{
    public GridCoordinate Location { get; set; }

    public Elf(GridCoordinate location)
    {
        Location = location;
    }
}
