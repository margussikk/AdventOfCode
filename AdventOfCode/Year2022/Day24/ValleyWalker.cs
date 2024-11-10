using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day24;

internal record ValleyWalker
{
    public GridCoordinate Coordinate { get; }
    public int Minute { get; }

    public ValleyWalker(GridCoordinate coordinate, int minute)
    {
        Coordinate = coordinate;
        Minute = minute;
    }
}
