using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day17;

internal class Crucible(GridCoordinate coordinate, GridDirection direction, int steps, int heatLoss)
{
    public GridCoordinate Coordinate { get; } = coordinate;

    public GridDirection Direction { get; private set; } = direction;

    public int Steps { get; private set; } = steps;

    public int HeatLoss { get; private set; } = heatLoss;

    public (GridCoordinate, GridDirection, int) State => (Coordinate, Direction, Steps);
}
