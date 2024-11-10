namespace AdventOfCode.Utilities.Geometry;

public class InfiniteBitGrid
{
    private BitGrid[,] _grid;

    private readonly int _subGridRowCount;
    private readonly int _subGridColumnCount;

    public InfiniteBitGrid()
    {
        _subGridRowCount = 64;
        _subGridColumnCount = 64;

        _grid = new BitGrid[,]
        {
            {
                new(_subGridRowCount, _subGridColumnCount)
            }
        };

        MinRow = 0;
        MaxRow = MinRow + _grid.GetLength(0) * _subGridRowCount - 1;

        MinColumn = 0;
        MaxColumn = MinColumn + _grid.GetLength(1) * _subGridColumnCount - 1;
    }

    public int MinRow { get; private set; }

    public int MaxRow { get; private set; }

    public int MinColumn { get; private set; }

    public int MaxColumn { get; private set; }

    public bool this[int row, int column]
    {
        get
        {
            if (row < MinRow || row > MaxRow || column < MinColumn || column > MaxColumn)
            {
                // Out of bounds is always 'false'
                return false;
            }

            var location = GetLocation(row, column);
            return _grid[location.GridRow, location.GridColumn][location.SubGridRow, location.SubGridColumn];
        }
        set
        {
            if (!value && (row < MinRow || row > MaxRow || column < MinColumn || column > MaxColumn))
            {
                // Out of bounds is already 'false'
                return;
            }

            AdjustGridSize(row, column);

            // Set value
            var location = GetLocation(row, column);
            _grid[location.GridRow, location.GridColumn][location.SubGridRow, location.SubGridColumn] = value;
        }
    }

    private void AdjustGridSize(int row, int column)
    {
        // Row
        if (row < MinRow || row > MaxRow)
        {
            int newGridMinRow;
            int newGridMaxRow;

            if (row < MinRow)
            {
                newGridMinRow = row / _subGridRowCount - 1;
                newGridMaxRow = _grid.GetLength(0) - 1;
            }
            else
            {
                newGridMinRow = 0;
                newGridMaxRow = row / _subGridRowCount;
            }

            var newGrid = new BitGrid[newGridMaxRow - newGridMinRow + 1, _grid.GetLength(1)];
            for (var gridRow = newGridMinRow; gridRow <= newGridMaxRow; gridRow++)
            {
                if (gridRow >= 0 && gridRow < _grid.GetLength(0))
                {
                    // Copy existing subgrids
                    for (var gridColumn = 0; gridColumn < _grid.GetLength(1); gridColumn++)
                    {
                        newGrid[gridRow - newGridMinRow, gridColumn] = _grid[gridRow, gridColumn];
                    }
                }
                else
                {
                    // Add empty subgrids
                    for (var gridColumn = 0; gridColumn < _grid.GetLength(1); gridColumn++)
                    {
                        newGrid[gridRow - newGridMinRow, gridColumn] = new BitGrid(_subGridRowCount, _subGridColumnCount);
                    }
                }
            }

            _grid = newGrid;
            MinRow = newGridMinRow * _subGridRowCount;
            MaxRow = MinRow + _grid.GetLength(0) * _subGridRowCount - 1;
        }

        // Column
        if (column < MinColumn || column > MaxColumn)
        {
            int newGridMinColumn;
            int newGridMaxColumn;

            if (column < MinColumn)
            {
                newGridMinColumn = column / _subGridColumnCount - 1;
                newGridMaxColumn = _grid.GetLength(1) - 1;
            }
            else
            {
                newGridMinColumn = 0;
                newGridMaxColumn = column / _subGridColumnCount;
            }

            var newGrid = new BitGrid[_grid.GetLength(0), newGridMaxColumn - newGridMinColumn + 1];
            for (var gridColumn = newGridMinColumn; gridColumn <= newGridMaxColumn; gridColumn++)
            {
                if (gridColumn >= 0 && gridColumn < _grid.GetLength(1))
                {
                    // Copy existing subgrids
                    for (var gridRow = 0; gridRow < _grid.GetLength(0); gridRow++)
                    {
                        newGrid[gridRow, gridColumn - newGridMinColumn] = _grid[gridRow, gridColumn];
                    }
                }
                else
                {
                    // Add empty subgrids
                    for (var gridRow = 0; gridRow < _grid.GetLength(0); gridRow++)
                    {
                        newGrid[gridRow, gridColumn - newGridMinColumn] = new BitGrid(_subGridRowCount, _subGridColumnCount);
                    }
                }
            }

            _grid = newGrid;
            MinColumn = newGridMinColumn * _subGridColumnCount;
            MaxColumn = MinColumn + _grid.GetLength(1) * _subGridColumnCount - 1;
        }
    }

    private InfiniteBitGridLocation GetLocation(int row, int column)
    {
        var normalizedRow = row - MinRow;
        var gridRow = normalizedRow / _subGridRowCount;
        var subGridRow = normalizedRow % _subGridRowCount;

        var normalizedColumn = column - MinColumn;
        var gridColumn = normalizedColumn / _subGridColumnCount;
        var subGridColumn = normalizedColumn % _subGridColumnCount;

        return new InfiniteBitGridLocation(gridRow, gridColumn, subGridRow, subGridColumn);
    }

    private sealed record InfiniteBitGridLocation(int GridRow, int GridColumn, int SubGridRow, int SubGridColumn);
}
