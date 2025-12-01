using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Utilities.PathFinding;

internal class GridPathWalker
{
    public GridPosition Position { get; set; }
    public GridPosition? PreviousPosition { get; set; }

    public List<GridCoordinate> Path { get; set; } = [];

    public int Cost { get; set; }

    public GridPosition[] MovementPositions()
    {
        return [Position.Left(), Position.Up(), Position.Right(), Position.Down()];
    }

    public GridPosition[] TurningPositions()
    {
        if (Position.Direction == GridDirection.None)
        {
            return
            [
                new GridPosition(Position.Coordinate.Left(), GridDirection.None),
                new GridPosition(Position.Coordinate.Up(), GridDirection.None),
                new GridPosition(Position.Coordinate.Right(), GridDirection.None),
                new GridPosition(Position.Coordinate.Down(), GridDirection.None),
            ];
        }
        else
        {
            return [Position.TurnLeft(), Position.Move(1), Position.TurnRight()];
        }
    }
}
