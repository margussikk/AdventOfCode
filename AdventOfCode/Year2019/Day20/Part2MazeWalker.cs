using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day20;

internal class Part2MazeWalker(int level, GridCoordinate coordinate, int steps)
{
    public int Level { get; } = level;
    public GridCoordinate Coordinate { get; } = coordinate;
    public int Steps { get; } = steps;
}
