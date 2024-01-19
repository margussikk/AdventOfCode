namespace AdventOfCode.Utilities.Geometry;

internal class GridWalker(GridCoordinate startCoordinate, GridCoordinate currentCoordinate, GridDirection direction, int steps)
{
    public GridCoordinate StartCoordinate { get; private set; } = startCoordinate;

    public GridCoordinate CurrentCoordinate { get; private set; } = currentCoordinate;

    public GridDirection Direction { get; private set; } = direction;

    public int Steps { get; private set; } = steps;

    public void Move(GridDirection direction, int steps = 1)
    {
        CurrentCoordinate = CurrentCoordinate.Move(direction, steps);
        Direction = direction;
        Steps += steps;
    }

    public void Step()
    {
        Move(Direction);
    }

    public void TurnLeft()
    {
        Move(Direction.TurnLeft());
    }

    public void TurnRight()
    {
        Move(Direction.TurnRight());
    }

    public GridWalker Clone()
    {
        return new GridWalker(StartCoordinate, CurrentCoordinate, Direction, Steps);
    }
}
