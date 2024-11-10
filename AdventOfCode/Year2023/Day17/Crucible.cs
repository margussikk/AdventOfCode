using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day17;

internal class Crucible
{
    public GridCoordinate Coordinate { get; }
    
    public GridDirection Direction { get; }
    
    public int Steps { get; }
    
    public int HeatLoss { get; }

    public Crucible(GridCoordinate coordinate, GridDirection direction, int steps, int heatLoss)
    {
        Coordinate = coordinate;
        Direction = direction;
        Steps = steps;
        HeatLoss = heatLoss;
    }

    public (GridCoordinate, GridDirection, int) State => (Coordinate, Direction, Steps);
}
