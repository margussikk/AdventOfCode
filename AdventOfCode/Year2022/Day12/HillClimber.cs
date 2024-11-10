using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day12;

internal class HillClimber
{
    public GridCoordinate Coordinate { get; }
    public int Steps { get; }

    public HillClimber(GridCoordinate coordinate, int steps)
    {
        Coordinate = coordinate;
        Steps = steps;
    }
}
