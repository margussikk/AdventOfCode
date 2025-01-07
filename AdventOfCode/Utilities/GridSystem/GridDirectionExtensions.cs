namespace AdventOfCode.Utilities.GridSystem;

internal static class GridDirectionExtensions
{
    public static GridDirection[] SideDirections(this GridDirection _)
    {
        return [GridDirection.Left, GridDirection.Up, GridDirection.Right, GridDirection.Down];
    }

    public static GridDirection[] AroundDirections(this GridDirection _)
    {
        return
        [
            GridDirection.UpLeft, GridDirection.Up, GridDirection.UpRight,
            GridDirection.Left, GridDirection.Right,
            GridDirection.DownLeft, GridDirection.Down, GridDirection.DownRight
        ];
    }

    public static GridDirection Flip(this GridDirection gridDirection)
    {
        var newGridDirection = GridDirection.None;

        var upDownDirection = gridDirection & GridDirection.UpAndDown;
        if (upDownDirection != GridDirection.None)
        {
            if (upDownDirection == GridDirection.UpAndDown)
            {
                newGridDirection |= GridDirection.UpAndDown;
            }
            else
            {
                newGridDirection |= (gridDirection ^ GridDirection.UpAndDown) & GridDirection.UpAndDown;
            }
        }

        var leftRightDirection = gridDirection & GridDirection.LeftAndRight;
        switch (leftRightDirection)
        {
            case GridDirection.None:
                return newGridDirection;
            case GridDirection.LeftAndRight:
                newGridDirection |= GridDirection.LeftAndRight;
                break;
            default:
                newGridDirection |= (gridDirection ^ GridDirection.LeftAndRight) & GridDirection.LeftAndRight;
                break;
        }


        return newGridDirection;
    }

    public static GridDirection Clear(this GridDirection gridDirection, GridDirection clearDirection)
    {
        return gridDirection & ~clearDirection;
    }

    public static GridDirection TurnLeft(this GridDirection gridDirection)
    {
        return gridDirection switch
        {
            GridDirection.Right => GridDirection.Up,
            GridDirection.Down => GridDirection.Right,
            GridDirection.Left => GridDirection.Down,
            GridDirection.Up => GridDirection.Left,
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }

    public static GridDirection TurnRight(this GridDirection gridDirection)
    {
        return gridDirection switch
        {
            GridDirection.Right => GridDirection.Down,
            GridDirection.Down => GridDirection.Left,
            GridDirection.Left => GridDirection.Up,
            GridDirection.Up => GridDirection.Right,
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }
}
