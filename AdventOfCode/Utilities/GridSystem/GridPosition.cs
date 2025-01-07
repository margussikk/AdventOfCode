using BenchmarkDotNet.Columns;

namespace AdventOfCode.Utilities.GridSystem;
internal readonly struct GridPosition : IEquatable<GridPosition>
{
    public GridCoordinate Coordinate { get; }

    public GridDirection Direction { get; }

    public GridPosition(GridCoordinate coordinate, GridDirection direction)
    {
        Coordinate = coordinate;
        Direction = direction;
    }

    public GridPosition Left() => new(Coordinate.Move(GridDirection.Left, 1), GridDirection.Left);
    public GridPosition Up() => new(Coordinate.Move(GridDirection.Up, 1), GridDirection.Up);
    public GridPosition Right() => new(Coordinate.Move(GridDirection.Right, 1), GridDirection.Right);
    public GridPosition Down() => new(Coordinate.Move(GridDirection.Down, 1), GridDirection.Down);

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

    public override string ToString()
    {
        return $"{Coordinate}, {Direction}";
    }
}
