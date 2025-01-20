namespace AdventOfCode.Utilities.GridSystem;
internal class WindowGrid<TObject> : GridBase<TObject>
{
    private readonly GridBase<TObject> _parentGrid;
    private readonly GridCoordinate _minCoordinate;

    public WindowGrid(GridBase<TObject> grid, GridCoordinate minCoordinate, GridCoordinate maxCoordinate)
    {
        if (!grid.InBounds(minCoordinate))
        {
            throw new InvalidOperationException("Min coordinate is not in bounds");
        }

        if (!grid.InBounds(maxCoordinate))
        {
            throw new InvalidOperationException("Max coordinate is not in bounds");
        }

        _parentGrid = grid;
        _minCoordinate = minCoordinate;

        FirstRow = 0;
        LastRow = maxCoordinate.Row - minCoordinate.Row;

        FirstColumn = 0;
        LastColumn = maxCoordinate.Column - minCoordinate.Column;
    }

    public override TObject this[int row, int column]
    {
        get => _parentGrid[_minCoordinate.Row + row, _minCoordinate.Column + column];
        set => _parentGrid[_minCoordinate.Row + row, _minCoordinate.Column + column] = value;
    }
}
