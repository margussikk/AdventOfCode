using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day21;

internal class Gardener
{
    public GridCoordinate Coordinate { get; }
    public int Steps { get; }

    public Gardener(GridCoordinate coordinate, int steps)
    {
        Coordinate = coordinate;
        Steps = steps;
    }
}
