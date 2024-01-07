using System.Collections;

namespace AdventOfCode.Utilities.Geometry;

internal class BitGrid
{
    private readonly BitArray _bitArray;

    public int Width { get; }

    public int Height { get; }

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public BitGrid(int rows, int columns)
    {
        Height = rows;
        Width = columns;

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
}

