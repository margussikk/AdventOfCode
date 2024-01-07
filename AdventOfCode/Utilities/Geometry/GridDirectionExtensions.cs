namespace AdventOfCode.Utilities.Geometry;

internal static class GridDirectionExtensions
{
    private static readonly GridDirection _upDownDirection = GridDirection.Up | GridDirection.Down;
    private static readonly GridDirection _leftRightDirection = GridDirection.Left | GridDirection.Right;

    public static GridDirection Flip(this GridDirection gridDirection)
    {
        GridDirection newGridDirection = GridDirection.None;

        var upDownDirection = gridDirection & _upDownDirection;
        if (upDownDirection != GridDirection.None)
        {
            if (upDownDirection == _upDownDirection)
            {
                newGridDirection |= _upDownDirection;
            }
            else
            {
                newGridDirection |= (gridDirection ^ _upDownDirection) & _upDownDirection;
            }
        }

        var leftRightDirection = gridDirection & _leftRightDirection;
        if (leftRightDirection != GridDirection.None)
        {
            if (leftRightDirection == _leftRightDirection)
            {
                newGridDirection |= _leftRightDirection;
            }
            else
            {
                newGridDirection |= (gridDirection ^ _leftRightDirection) & _leftRightDirection;
            }
        }


        return newGridDirection;
    }
}
