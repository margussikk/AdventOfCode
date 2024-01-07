namespace AdventOfCode.Utilities.Geometry;

internal class GridWalker(GridCoordinate startCoordinate, GridCoordinate currentCoordinate, GridDirection direction, int steps)
{
    public GridCoordinate StartCoordinate { get; private set; } = startCoordinate;

    public GridCoordinate CurrentCoordinate { get; private set; } = currentCoordinate;

    public GridDirection Direction { get; private set; } = direction;

    public int Steps { get; private set; } = steps;

    public void MoveTo(GridDirection direction, int steps = 1)
    {
        CurrentCoordinate = CurrentCoordinate.MoveTo(direction, steps);
        Direction = direction;
        Steps += steps;
    }

    public void Step()
    {
        MoveTo(Direction);
    }

    public void TurnLeft()
    {
        switch (Direction)
        {
            case GridDirection.Right:
                MoveTo(GridDirection.Up);
                break;
            case GridDirection.Left:
                MoveTo(GridDirection.Down);
                break;
            case GridDirection.Down:
                MoveTo(GridDirection.Right);
                break;
            case GridDirection.Up:
                MoveTo(GridDirection.Left);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public void TurnRight()
    {
        switch (Direction)
        {
            case GridDirection.Left:
                MoveTo(GridDirection.Up);
                break;
            case GridDirection.Right:
                MoveTo(GridDirection.Down);
                break;
            case GridDirection.Down:
                MoveTo(GridDirection.Left);
                break;
            case GridDirection.Up:
                MoveTo(GridDirection.Right);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public GridWalker Clone()
    {
        return new GridWalker(StartCoordinate, CurrentCoordinate, Direction, Steps);
    }
}
