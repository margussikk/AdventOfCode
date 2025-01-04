using AdventOfCode.Utilities.Geometry;
using System.Collections;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Utilities.Simulation;
internal class GameOfLife<T> : IEnumerable<GameOfLifeCell<T>> where T : struct, Enum
{
    private readonly int[,] _array;

    public int Width => _array.GetLength(1);

    public int Height => _array.GetLength(0);

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public GameOfLife(int height, int width)
    {
        _array = new int[height, width];
    }

    public GameOfLife<T> Clone()
    {
        var grid = new GameOfLife<T>(Height, Width);

        Array.Copy(_array, grid._array, _array.Length);

        return grid;
    }

    public bool InBounds(GridCoordinate coordinate)
    {
        return coordinate.Row >= 0 && coordinate.Row < Height &&
               coordinate.Column >= 0 && coordinate.Column < Width;
    }

    public GameOfLifeCell<T> this[GridCoordinate coordinate]
    {
        get => new(coordinate, _array[coordinate.Row, coordinate.Column]);
    }

    public void SetState(GridCoordinate coordinate, T @object)
    {
        if (!InBounds(coordinate))
        {
            throw new InvalidOperationException("Coordinate is out of bounds");
        }

        // Remove old
        var subtract = 0;
        var currentObjectInt = _array[coordinate.Row, coordinate.Column] & 0x0F;
        if (currentObjectInt != 0)
        {
            subtract = 1 << (4 * currentObjectInt);
        }

        // Add new
        var add = 0;
        var nextObjectInt = Unsafe.BitCast<T, int>(@object);
        if (nextObjectInt != 0)
        {
            add = 1 << (4 * nextObjectInt);
        }

        if (currentObjectInt != nextObjectInt)
        {
            foreach (var neighborCoordinate in coordinate.AroundNeighbors().Where(InBounds))
            {
                _array[neighborCoordinate.Row, neighborCoordinate.Column] = _array[neighborCoordinate.Row, neighborCoordinate.Column] - subtract + add;
            }
        
            _array[coordinate.Row, coordinate.Column] = (_array[coordinate.Row, coordinate.Column] & 0x7FFF_FFF0) + nextObjectInt;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is GameOfLife<T> gameOfLife &&
               Height == gameOfLife.Height &&
               Width == gameOfLife.Width &&
               EqualityComparer<int[,]>.Default.Equals(_array, gameOfLife._array);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                hashCode.Add(_array[row, column]);
            }
        }

        return hashCode.ToHashCode();
    }

    public IEnumerator<GameOfLifeCell<T>> GetEnumerator()
    {
        for (var row = 0; row < Height; row++)
        {
            for (var column = 0; column < Width; column++)
            {
                yield return new GameOfLifeCell<T>(new GridCoordinate(row, column), _array[row, column]);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
