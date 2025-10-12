namespace AdventOfCode.Utilities.GridSystem;

internal class GridRegion
{
    public int FirstRow { get; }
    public int LastRow { get; }

    public int FirstColumn { get; }
    public int LastColumn { get; }

    public GridRegion(GridCoordinate topLeftCoordinate, GridCoordinate bottomRightCoordinate)
    {
        FirstRow = topLeftCoordinate.Row;
        FirstColumn = topLeftCoordinate.Column;
        LastRow = bottomRightCoordinate.Row;
        LastColumn = bottomRightCoordinate.Column;
    }

    public virtual bool InBounds(GridCoordinate coordinate)
    {
        return coordinate.Row >= FirstRow && coordinate.Row <= LastRow &&
               coordinate.Column >= FirstColumn && coordinate.Column <= LastColumn;
    }
}
