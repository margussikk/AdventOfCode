namespace AdventOfCode.Utilities.GridSystem;

internal class GridWalker
{
    public GridCoordinate StartCoordinate { get; }

    public GridCoordinate Coordinate { get; private set; }

    public GridDirection Direction { get; private set; }

    public int Steps { get; private set; }

    public List<GridCoordinate> Breadcrumbs { get; private set; } = [];

    public GridWalker(GridCoordinate startCoordinate, GridCoordinate currentCoordinate, GridDirection direction, int steps)
    {
        StartCoordinate = startCoordinate;
        Coordinate = currentCoordinate;
        Direction = direction;
        Steps = steps;
    }

    public GridWalker(GridPosition position)
    {
        StartCoordinate = position.Coordinate;
        Coordinate = position.Coordinate;
        Direction = position.Direction;
        Steps = 0;
    }

    public void Move(GridDirection direction, int steps = 1)
    {
        Coordinate = Coordinate.Move(direction, steps);
        Direction = direction;
        Steps += steps;
    }

    public void Step()
    {
        Move(Direction);
    }

    public void TurnLeft()
    {
        Direction = Direction.TurnLeft();
    }

    public void MoveLeft()
    {
        Direction.TurnLeft();
        Step();
    }

    public void MoveRight()
    {
        Direction.TurnRight();
        Step();
    }

    public void TurnRight()
    {
        Direction = Direction.TurnRight();
    }

    public GridWalker Clone()
    {
        return new GridWalker(StartCoordinate, Coordinate, Direction, Steps);
    }
}
