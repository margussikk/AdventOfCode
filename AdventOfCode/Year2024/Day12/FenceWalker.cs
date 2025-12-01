using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2024.Day12;

internal class FenceWalker
{
    private readonly GridCoordinate _startCoordinate;
    private readonly GridDirection _startDirection;

    public GridCoordinate Coordinate { get; private set; }
    public GridDirection Direction { get; private set; }

    public GridDirection FenceDirection { get; private set; }

    public bool IsWalking { get; set; }

    public bool IsAtStart => Coordinate == _startCoordinate && Direction == _startDirection;

    public int Turns { get; private set; }

    public FenceWalker(GridCoordinate startCoordinate, GridDirection startDirection)
    {
        _startCoordinate = startCoordinate;
        _startDirection = startDirection;

        Coordinate = startCoordinate;
        Direction = startDirection;

        FenceDirection = startDirection.TurnLeft();
    }

    public void Step()
    {
        Coordinate = Coordinate.Move(Direction);
    }

    public void TurnLeft()
    {
        FenceDirection = FenceDirection.TurnLeft();
        Direction = Direction.TurnLeft();

        Turns++;
    }

    public void TurnRight()
    {
        FenceDirection = FenceDirection.TurnRight();
        Direction = Direction.TurnRight();

        Turns++;
    }
}
