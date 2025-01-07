using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2021.Day15;

internal class CaveWalker
{
    public GridCoordinate Coordinate { get; }
    public int Distance { get; }

    public CaveWalker(GridCoordinate coordinate, int distance)
    {
        Coordinate = coordinate;
        Distance = distance;
    }
}
