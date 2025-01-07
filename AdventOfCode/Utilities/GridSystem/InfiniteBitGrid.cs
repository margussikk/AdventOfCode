using System.Collections;

namespace AdventOfCode.Utilities.GridSystem;

internal class InfiniteBitGrid : IGrid<bool>
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

    public int Width => int.MaxValue;

    public int Height => int.MaxValue;

    public int LastRowIndex => MaxRow;

    public int LastColumnIndex => MaxColumn;

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

    public bool InBounds(GridCoordinate coordinate) => true; // It's infinite

    public bool this[GridCoordinate coordinate]
    {
        get => this[coordinate.Row, coordinate.Column];
        set => this[coordinate.Row, coordinate.Column] = value;
    }

    public GridCell<bool> Cell(GridCoordinate coordinate)
    {
        return new GridCell<bool>(coordinate, this[coordinate.Row, coordinate.Column]);
    }

    public IEnumerable<GridCell<bool>> SideNeighbors(GridCoordinate coordinate)
    {
        foreach (var neighborCoordinate in coordinate.SideNeighbors())
        {

            yield return new GridCell<bool>(neighborCoordinate, this[neighborCoordinate]);
        }
    }

    public IEnumerator<GridCell<bool>> GetEnumerator()
    {
        throw new NotImplementedException("Cannot enumerate over infinite space");
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void AdjustGridSize(int row, int column)
    {
        // Row
        if (row < MinRow || row > MaxRow)
        {
            var emptyRowsBefore = 0;
            var emptyRowsAfter = 0;

            if (row < MinRow)
            {
                emptyRowsBefore = (MinRow - row) / _subGridRowCount + 1;
            }
            else if (row > MaxRow)
            {
                emptyRowsAfter = (row - MaxRow) / _subGridRowCount + 1;
            }

            var newGrid = new BitGrid[emptyRowsBefore + _grid.GetLength(0) + emptyRowsAfter, _grid.GetLength(1)];

            // Add empty subgrids before
            for (var subGridRow = 0; subGridRow < emptyRowsBefore; subGridRow++)
            {
                for (var subGridColumn = 0; subGridColumn < _grid.GetLength(1); subGridColumn++)
                {
                    newGrid[subGridRow, subGridColumn] = new BitGrid(_subGridRowCount, _subGridColumnCount);
                }
            }
            var subGridRowOffset = emptyRowsBefore;

            // Copy existing subgrids
            for (var subGridRow = 0; subGridRow < _grid.GetLength(0); subGridRow++)
            {
                for (var subGridColumn = 0; subGridColumn < _grid.GetLength(1); subGridColumn++)
                {
                    newGrid[subGridRow + subGridRowOffset, subGridColumn] = _grid[subGridRow, subGridColumn];
                }
            }
            subGridRowOffset += _grid.GetLength(0);

            // Add empty subgrids after
            for (var subGridRow = 0; subGridRow < emptyRowsAfter; subGridRow++)
            {
                for (var subGridColumn = 0; subGridColumn < _grid.GetLength(1); subGridColumn++)
                {
                    newGrid[subGridRow + subGridRowOffset, subGridColumn] = new BitGrid(_subGridRowCount, _subGridColumnCount);
                }
            }

            _grid = newGrid;
            MinRow -= emptyRowsBefore * _subGridRowCount;
            MaxRow += emptyRowsAfter * _subGridRowCount;
        }

        // Column
        if (column < MinColumn || column > MaxColumn)
        {
            var emptyColumnsBefore = 0;
            var emptyColumnsAfter = 0;

            if (column < MinColumn)
            {
                emptyColumnsBefore = (MinColumn - column) / _subGridColumnCount + 1;
            }
            else if (column > MaxColumn)
            {
                emptyColumnsAfter = (column - MaxColumn) / _subGridColumnCount + 1;
            }

            var newGrid = new BitGrid[_grid.GetLength(0), emptyColumnsBefore + _grid.GetLength(1) + emptyColumnsAfter];

            // Add empty subgrids before
            for (var subGridColumn = 0; subGridColumn < emptyColumnsBefore; subGridColumn++)
            {
                for (var subGridRow = 0; subGridRow < _grid.GetLength(0); subGridRow++)
                {
                    newGrid[subGridRow, subGridColumn] = new BitGrid(_subGridRowCount, _subGridColumnCount);
                }
            }
            var subGridColumnOffset = emptyColumnsBefore;

            // Copy existing subgrids
            for (var subGridColumn = 0; subGridColumn < _grid.GetLength(1); subGridColumn++)
            {
                for (var subGridRow = 0; subGridRow < _grid.GetLength(0); subGridRow++)
                {
                    newGrid[subGridRow, subGridColumnOffset + subGridColumn] = _grid[subGridRow, subGridColumn];
                }
            }
            subGridColumnOffset += _grid.GetLength(1);

            // Add empty subgrids after
            for (var subGridColumn = 0; subGridColumn < emptyColumnsAfter; subGridColumn++)
            {
                for (var subGridRow = 0; subGridRow < _grid.GetLength(0); subGridRow++)
                {
                    newGrid[subGridRow, subGridColumnOffset + subGridColumn] = new BitGrid(_subGridRowCount, _subGridColumnCount);
                }
            }

            _grid = newGrid;
            MinColumn -= emptyColumnsBefore * _subGridColumnCount;
            MaxColumn += emptyColumnsAfter * _subGridColumnCount;
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
