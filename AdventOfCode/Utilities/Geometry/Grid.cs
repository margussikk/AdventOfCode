using System.Collections;

namespace AdventOfCode.Utilities.Geometry;

internal class Grid<T>(int rows, int columns): IEnumerable<GridCell<T>>
{
    private readonly T[,] _array = new T[rows, columns];

    public int Width => _array.GetLength(1);

    public int Height => _array.GetLength(0);

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public T this[int row, int column]
    {
        get => _array[row, column];
        set => _array[row, column] = value;
    }

    public T this[GridCoordinate coordinate]
    {
        get => this[coordinate.Row, coordinate.Column];
        set => this[coordinate.Row, coordinate.Column] = value;
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

    public GridCoordinate? FindCoordinate(Func<T, bool> predicate)
    {
        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                if (predicate(_array[row, column]))
                {
                    return new GridCoordinate(row, column);
                }
            }
        }

        return null;
    }

    public int Count(Func<T, bool> predicate)
    {
        var count = 0;

        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                if (predicate(_array[row, column]))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public IEnumerable<GridCell<T>> Row(int row)
    {
        for (var column = 0; column < Width; column++)
        {
            yield return new GridCell<T>(_array[row, column], new GridCoordinate(row, column));
        }
    }

    public IEnumerable<GridCell<T>> Column(int column)
    {
        for (var row = 0; row < Height; row++)
        {
            yield return new GridCell<T>(_array[row, column], new GridCoordinate(row, column));
        }
    }

    public IEnumerable<GridCell<T>> Sides(GridCoordinate coordinate, GridDirection direction = GridDirection.Up | GridDirection.Down | GridDirection.Left | GridDirection.Right)
    {
        var sideCoordinates = new List<GridCoordinate>();

        if ((direction & GridDirection.Up) != GridDirection.None)
        {
            sideCoordinates.Add(new GridCoordinate(coordinate.Row - 1, coordinate.Column));
        }

        if ((direction & GridDirection.Down) != GridDirection.None)
        {
            sideCoordinates.Add(new GridCoordinate(coordinate.Row + 1, coordinate.Column));
        }

        if ((direction & GridDirection.Left) != GridDirection.None)
        {
            sideCoordinates.Add(new GridCoordinate(coordinate.Row, coordinate.Column - 1));
        }

        if ((direction & GridDirection.Right) != GridDirection.None)
        {
            sideCoordinates.Add(new GridCoordinate(coordinate.Row, coordinate.Column + 1));
        }

        foreach(var sideCoordinate in sideCoordinates)
        {
            if (InBounds(sideCoordinate))
            {
                yield return new GridCell<T>(_array[sideCoordinate.Row, sideCoordinate.Column], sideCoordinate);
            }
        }
    }

    public Grid<T> Clone()
    {
        var grid = new Grid<T>(Height, Width);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = _array[row, column];
            }
        }

        return grid;
    }

    public Grid<T> RotateClockwise()
    {
        var grid = new Grid<T>(Width, Height);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = _array[grid.LastColumnIndex - column, row];
            }
        }

        return grid;
    }

    public Grid<T> RotateCounterClockwise()
    {
        var grid = new Grid<T>(Width, Height);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                grid[row, column] = _array[column, grid.LastRowIndex - row];
            }
        }

        return grid;
    }

    public override bool Equals(object? obj)
    {
        return obj is Grid<T> grid &&
               Height == grid.Height &&
               Width == grid.Width &&
               EqualityComparer<T[,]>.Default.Equals(_array, grid._array);
    }
    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                hashCode.Add(_array[row, column]!.GetHashCode());
            }
        }

        return hashCode.ToHashCode();
    }

    public IEnumerator<GridCell<T>> GetEnumerator()
    {
        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                yield return new GridCell<T>(_array[row, column], new GridCoordinate(row, column));
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
