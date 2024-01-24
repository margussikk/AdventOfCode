using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2021.Day15;

internal class CaveWalker(GridCoordinate coordinate, int distance)
{
    public GridCoordinate Coordinate { get; } = coordinate;
    public int Distance { get; } = distance;
}
