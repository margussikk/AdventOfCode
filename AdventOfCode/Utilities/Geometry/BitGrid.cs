using System.Collections;
using System.Text;

namespace AdventOfCode.Utilities.Geometry;

internal class BitGrid : IGrid<bool>
{
    private readonly BitArray _bitArray;

    public int Width { get; }

    public int Height { get; }

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public int Area => Width * Height;

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

    public IEnumerable<GridCell<bool>> Row(int row)
    {
        for (var column = 0; column <= LastColumnIndex; column++)
        {
            yield return new GridCell<bool>(new GridCoordinate(row, column), _bitArray.Get(row * Width + column));
        }
    }

    public IEnumerable<GridCell<bool>> Column(int column)
    {
        for (var row = 0; row <= LastRowIndex; row++)
        {
            yield return new GridCell<bool>(new GridCoordinate(row, column), _bitArray.Get(row * Width + column));
        }
    }

    public GridCell<bool> Cell(GridCoordinate coordinate)
    {
        return new GridCell<bool>(coordinate, _bitArray.Get(coordinate.Row * Width + coordinate.Column));
    }

    public IEnumerable<GridCell<bool>> SideNeighbors(GridCoordinate coordinate)
    {
        foreach (var neighborCoordinate in coordinate.SideNeighbors().Where(InBounds))
        {
            yield return new GridCell<bool>(neighborCoordinate, _bitArray.Get(neighborCoordinate.Row * Width + neighborCoordinate.Column));
        }
    }

    public BitGrid Clone()
    {
        var grid = new BitGrid(Height, Width);

        for (var index = 0; index < _bitArray.Length; index++)
        {
            grid._bitArray.Set(index, _bitArray.Get(index));
        }

        return grid;
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
                Console.Write(value ? '#' : ' ');
            }
            Console.WriteLine();
        }
    }

    public void PrintToFile(string path)
    {
        var stringBuilder = new StringBuilder();

        for (var row = 0; row <= LastRowIndex; row++)
        {
            for (var column = 0; column <= LastColumnIndex; column++)
            {
                var character = _bitArray.Get(row * Width + column) ? '#': ' ';
                stringBuilder.Append(character);
            }
            stringBuilder.AppendLine();
        }

        File.WriteAllText(path, stringBuilder.ToString());
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

