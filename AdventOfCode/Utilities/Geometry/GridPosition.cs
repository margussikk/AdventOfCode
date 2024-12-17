namespace AdventOfCode.Utilities.Geometry;
internal readonly struct GridPosition : IEquatable<GridPosition>
{
    public GridCoordinate Coordinate { get; }

    public GridDirection Direction { get; }

    public GridPosition(GridCoordinate coordinate, GridDirection direction)
    {
        Coordinate = coordinate;
        Direction = direction;
    }

    public GridPosition Move(int steps) => new(Coordinate.Move(Direction, steps), Direction);

    public GridPosition TurnLeft() => new(Coordinate, Direction.TurnLeft());

    public GridPosition TurnRight() => new(Coordinate, Direction.TurnRight());

    public static bool operator ==(GridPosition position1, GridPosition position2)
    {
        return position1.Equals(position2);
    }

    public static bool operator !=(GridPosition position1, GridPosition position2)
    {
        return !position1.Equals(position2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Coordinate, Direction);
    }

    public bool Equals(GridPosition other)
    {
        return Coordinate == other.Coordinate && Direction == other.Direction;
    }

    public override bool Equals(object? obj)
    {
        return obj is GridPosition gridPosition && Equals(gridPosition);
    }

}
