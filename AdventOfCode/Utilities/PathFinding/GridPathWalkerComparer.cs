namespace AdventOfCode.Utilities.PathFinding;

internal class GridPathWalkerComparer : IComparer<GridPathWalker>
{
    public int Compare(GridPathWalker? first, GridPathWalker? second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        var firstPreviousCoordinate = first.PreviousPosition!.Value.Coordinate;
        var secondPreviousCoordinate = second.PreviousPosition!.Value.Coordinate;

        var compare = firstPreviousCoordinate.Row.CompareTo(secondPreviousCoordinate.Row);
        if (compare != 0)
        {
            return compare;
        }

        compare = firstPreviousCoordinate.Column.CompareTo(secondPreviousCoordinate.Column);
        if (compare != 0)
        {
            return compare;
        }

        return 0;
    }
}
