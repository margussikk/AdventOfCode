using System.Collections;

namespace AdventOfCode.Utilities.Geometry;

internal class BitGrid : IEnumerable<GridCell<bool>>
{
    private readonly BitArray _bitArray;

    public int Width { get; }

    public int Height { get; }

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public BitGrid(int height, int width)
    {
        Height = height;
        Width = width;

        _bitArray = new BitArray(Height * Width);
    }

    public bool this[int row, int column]
    {
        get => _bitArray.Get(row * Width + column);
        set => _bitArray.Set(row * Width + column, value);
    }

    public bool this[GridCoordinate gridCoordinate]
    {
        get => this[gridCoordinate.Row, gridCoordinate.Column];
        set => this[gridCoordinate.Row, gridCoordinate.Column] = value;
    }

    public bool InBounds(int row, int column)
    {
        return row >= 0 && row < Height &&
               column >= 0 && column < Width;
    }

    public bool InBounds(GridCoordinate coordinate)
    {
        return InBounds(coordinate.Row, coordinate.Column);
    }

    public BitGrid RotateClockwise()
    {
        var grid = new BitGrid(Width, Height);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = _bitArray.Get((grid.LastColumnIndex - column) * Width + row);
            }
        }

        return grid;
    }

    public BitGrid RotateCounterClockwise()
    {
        var grid = new BitGrid(Width, Height);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = _bitArray.Get(column * Width + grid.LastRowIndex - row);
            }
        }

        return grid;
    }

    public BitGrid FlipHorizontally()
    {
        var grid = new BitGrid(Height, Width);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = _bitArray.Get(row * Width + grid.LastColumnIndex - column);
            }
        }

        return grid;
    }

    public BitGrid FlipVertically()
    {
        var grid = new BitGrid(Height, Width);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = _bitArray.Get((grid.LastRowIndex - row) * Width + column);
            }
        }

        return grid;
    }

    public void Print()
    {
        for (var row = 0; row <= LastRowIndex; row++)
        {
            for (var column = 0; column <= LastColumnIndex; column++)
            {
                var value = _bitArray.Get(row * Width + column);
                if (value)
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write(' ');
                }
            }
            Console.WriteLine();
        }
    }

    public IEnumerator<GridCell<bool>> GetEnumerator()
    {
        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                yield return new GridCell<bool>(new GridCoordinate(row, column), _bitArray.Get(row * Width + column));
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

