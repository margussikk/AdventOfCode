using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2019.Day20;

internal class Part2MazeWalker
{
    public int Level { get; }
    public GridCoordinate Coordinate { get; }
    public int Steps { get; }

    public Part2MazeWalker(int level, GridCoordinate coordinate, int steps)
    {
        Level = level;
        Coordinate = coordinate;
        Steps = steps;
    }
}
