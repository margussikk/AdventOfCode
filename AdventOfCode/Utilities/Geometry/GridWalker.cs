namespace AdventOfCode.Utilities.Geometry;

internal class GridWalker
{
    public GridCoordinate StartCoordinate { get; }

    public GridCoordinate CurrentCoordinate { get; private set; }

    public GridDirection Direction { get; private set; }

    public int Steps { get; private set; }

    public List<GridCoordinate> Breadcrumbs { get; private set; } = [];

    public GridWalker(GridCoordinate startCoordinate, GridCoordinate currentCoordinate, GridDirection direction, int steps)
    {
        StartCoordinate = startCoordinate;
        CurrentCoordinate = currentCoordinate;
        Direction = direction;
        Steps = steps;
    }

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
