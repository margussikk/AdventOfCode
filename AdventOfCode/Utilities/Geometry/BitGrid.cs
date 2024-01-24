using System.Collections;

namespace AdventOfCode.Utilities.Geometry;

internal class BitGrid
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
}

