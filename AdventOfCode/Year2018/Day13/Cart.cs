using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2018.Day13;
internal class Cart
{
    public GridCoordinate Coordinate { get; private set; }

    public GridDirection Direction { get; private set; }

    public GridDirection NextTurn { get; private set; } = GridDirection.Left;

    public bool Crashed { get; set; }

    public Cart(GridCoordinate coordinate, GridDirection direction)
    {
        Coordinate = coordinate;
        Direction = direction;
    }

    public void Move(Grid<GridDirection> grid)
    {
        Coordinate = Coordinate.Move(Direction);

        if (grid[Coordinate] == GridDirection.DownAndLeft)
        {
            Direction = Direction switch
            {
                GridDirection.Left or GridDirection.Right => Direction.TurnRight(),
                GridDirection.Up or GridDirection.Down => Direction.TurnLeft(),
                _ => throw new NotImplementedException()
            };
        }
        else if (grid[Coordinate] == GridDirection.UpAndLeft)
        {
            Direction = Direction switch
            {
                GridDirection.Left or GridDirection.Right => Direction.TurnLeft(),
                GridDirection.Up or GridDirection.Down => Direction.TurnRight(),
                _ => throw new NotImplementedException()
            };
        }
        else if (grid[Coordinate] == GridDirection.AllSides)
        {
            if (NextTurn == GridDirection.Left)
            {
                Direction = Direction.TurnLeft();
            }
            else if (NextTurn == GridDirection.Right)
            {
                Direction = Direction.TurnRight();
            }

            NextTurn = NextTurn switch
            {
                GridDirection.Left => GridDirection.None,
                GridDirection.None => GridDirection.Right,
                GridDirection.Right => GridDirection.Left,
                _ => throw new NotImplementedException()
            };
        }
    }
}
