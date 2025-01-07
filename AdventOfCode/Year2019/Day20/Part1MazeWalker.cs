using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2019.Day20;

internal class Part1MazeWalker
{
    public GridCoordinate Coordinate { get; }
    public int Steps { get; }

    public Part1MazeWalker(GridCoordinate coordinate, int steps)
    {
        Coordinate = coordinate;
        Steps = steps;
    }
}
