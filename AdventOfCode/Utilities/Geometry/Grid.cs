using System.Collections;
using System.Text;

namespace AdventOfCode.Utilities.Geometry;

internal class Grid<T>(int height, int width) : IEnumerable<GridCell<T>>
{
    private readonly T[,] _array = new T[height, width];

    public int Width => _array.GetLength(1);

    public int Height => _array.GetLength(0);

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public int Area => Width * Height;

    public T this[int row, int column]
    {
        get => _array[row, column];
        set => _array[row, column] = value;
    }

    public T this[GridCoordinate coordinate]
    {
        get => _array[coordinate.Row, coordinate.Column];
        set => _array[coordinate.Row, coordinate.Column] = value;
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

    public IEnumerable<GridCell<T>> Row(int row)
    {
        for (var column = 0; column < Width; column++)
        {
            yield return new GridCell<T>(new GridCoordinate(row, column), _array[row, column]);
        }
    }

    public IEnumerable<GridCell<T>> Column(int column)
    {
        for (var row = 0; row < Height; row++)
        {
            yield return new GridCell<T>(new GridCoordinate(row, column), _array[row, column]);
        }
    }

    public IEnumerable<GridCell<T>> SideNeighbors(GridCoordinate coordinate, GridDirection direction = GridDirection.AllSides)
    {
        var neighborCoordinates = new List<GridCoordinate>();

        if ((direction & GridDirection.Up) != GridDirection.None)
        {
            neighborCoordinates.Add(new GridCoordinate(coordinate.Row - 1, coordinate.Column));
        }

        if ((direction & GridDirection.Down) != GridDirection.None)
        {
            neighborCoordinates.Add(new GridCoordinate(coordinate.Row + 1, coordinate.Column));
        }

        if ((direction & GridDirection.Left) != GridDirection.None)
        {
            neighborCoordinates.Add(new GridCoordinate(coordinate.Row, coordinate.Column - 1));
        }

        if ((direction & GridDirection.Right) != GridDirection.None)
        {
            neighborCoordinates.Add(new GridCoordinate(coordinate.Row, coordinate.Column + 1));
        }

        foreach (var neighborCoordinate in neighborCoordinates.Where(InBounds))
        {
            yield return new GridCell<T>(neighborCoordinate, this[neighborCoordinate]);
        }
    }

    public IEnumerable<GridCell<T>> AroundNeighbors(GridCoordinate coordinate)
    {
        foreach (var neighborCoordinate in coordinate.AroundNeighbors().Where(InBounds))
        {
            yield return new GridCell<T>(neighborCoordinate, this[neighborCoordinate]);
        }
    }

    public Grid<T> Clone()
    {
        var grid = new Grid<T>(Height, Width);

        Array.Copy(_array, grid._array, _array.Length);

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
                yield return new GridCell<T>(new GridCoordinate(row, column), _array[row, column]);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Print(Func<T, char> mapper)
    {
        var stringBuilder = new StringBuilder();

        for (var row = 0; row <= LastRowIndex; row++)
        {
            for (var column = 0; column <= LastColumnIndex; column++)
            {
                var character = mapper(_array[row, column]);
                stringBuilder.Append(character);
            }
            stringBuilder.AppendLine();
        }

        Console.WriteLine(stringBuilder.ToString());
    }

    public void PrintToFile(string path, Func<T, char> mapper)
    {
        var stringBuilder = new StringBuilder();

        for (var row = 0; row <= LastRowIndex; row++)
        {
            for (var column = 0; column <= LastColumnIndex; column++)
            {
                var character = mapper(_array[row, column]);
                stringBuilder.Append(character);
            }
            stringBuilder.AppendLine();
        }

        File.WriteAllText(path, stringBuilder.ToString());
    }
}
