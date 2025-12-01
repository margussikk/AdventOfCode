namespace AdventOfCode.Utilities.GridSystem;

internal class InfiniteGrid<TObject> : GridBase<TObject>
{
    private const int SubGridSize = 64;
    private Grid<TObject>[,] _grid;

    public InfiniteGrid()
    {
        _grid = new Grid<TObject>[,]
        {
            {
                new(SubGridSize, SubGridSize)
            }
        };

        FirstRow = 0;
        LastRow = FirstRow + _grid.GetLength(0) * SubGridSize - 1;

        FirstColumn = 0;
        LastColumn = FirstColumn + _grid.GetLength(1) * SubGridSize - 1;
    }

    public override bool InBounds(int row, int column) => true; // It's infinite

    public override TObject this[int row, int column]
    {
        get
        {
            if (row < FirstRow || row > LastRow || column < FirstColumn || column > LastColumn)
            {
                // Out of bounds is always default
                return default!;
            }

            var location = GetLocation(row, column);
            return _grid[location.GridRow, location.GridColumn][location.SubGridRow, location.SubGridColumn];
        }
        set
        {
            if (value is null && (row < FirstRow || row > LastRow || column < FirstColumn || column > LastColumn))
            {
                // Out of bounds is already null
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
        if (row < FirstRow || row > LastRow)
        {
            var emptyRowsBefore = 0;
            var emptyRowsAfter = 0;

            if (row < FirstRow)
            {
                emptyRowsBefore = (FirstRow - row) / SubGridSize + 1;
            }
            else if (row > LastRow)
            {
                emptyRowsAfter = (row - LastRow) / SubGridSize + 1;
            }

            var newGrid = new Grid<TObject>[emptyRowsBefore + _grid.GetLength(0) + emptyRowsAfter, _grid.GetLength(1)];

            // Add empty subgrids before
            for (var subGridRow = 0; subGridRow < emptyRowsBefore; subGridRow++)
            {
                for (var subGridColumn = 0; subGridColumn < _grid.GetLength(1); subGridColumn++)
                {
                    newGrid[subGridRow, subGridColumn] = new Grid<TObject>(SubGridSize, SubGridSize);
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
                    newGrid[subGridRow + subGridRowOffset, subGridColumn] = new Grid<TObject>(SubGridSize, SubGridSize);
                }
            }

            _grid = newGrid;
            FirstRow -= emptyRowsBefore * SubGridSize;
            LastRow += emptyRowsAfter * SubGridSize;
        }

        // Column
        if (column < FirstColumn || column > LastColumn)
        {
            var emptyColumnsBefore = 0;
            var emptyColumnsAfter = 0;

            if (column < FirstColumn)
            {
                emptyColumnsBefore = (FirstColumn - column) / SubGridSize + 1;
            }
            else if (column > LastColumn)
            {
                emptyColumnsAfter = (column - LastColumn) / SubGridSize + 1;
            }

            var newGrid = new Grid<TObject>[_grid.GetLength(0), emptyColumnsBefore + _grid.GetLength(1) + emptyColumnsAfter];

            // Add empty subgrids before
            for (var subGridColumn = 0; subGridColumn < emptyColumnsBefore; subGridColumn++)
            {
                for (var subGridRow = 0; subGridRow < _grid.GetLength(0); subGridRow++)
                {
                    newGrid[subGridRow, subGridColumn] = new Grid<TObject>(SubGridSize, SubGridSize);
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
                    newGrid[subGridRow, subGridColumnOffset + subGridColumn] = new Grid<TObject>(SubGridSize, SubGridSize);
                }
            }

            _grid = newGrid;
            FirstColumn -= emptyColumnsBefore * SubGridSize;
            LastColumn += emptyColumnsAfter * SubGridSize;
        }
    }

    private InfiniteGridLocation GetLocation(int row, int column)
    {
        var gridRow = Math.DivRem(row - FirstRow, SubGridSize, out var subGridRow);
        var gridColumn = Math.DivRem(column - FirstColumn, SubGridSize, out var subGridColumn);

        return new InfiniteGridLocation(gridRow, gridColumn, subGridRow, subGridColumn);
    }

    private sealed record InfiniteGridLocation(int GridRow, int GridColumn, int SubGridRow, int SubGridColumn);
}
