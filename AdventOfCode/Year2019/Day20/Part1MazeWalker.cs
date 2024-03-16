using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day20;

internal class Part1MazeWalker(GridCoordinate coordinate, int steps)
{
    public GridCoordinate Coordinate { get; } = coordinate;
    public int Steps { get; } = steps;
}
