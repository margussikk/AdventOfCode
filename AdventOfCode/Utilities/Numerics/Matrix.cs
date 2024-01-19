using System.Numerics;

namespace AdventOfCode.Utilities.Numerics;

internal class Matrix<T>(int rows, int columns) where T : INumber<T>
{
    private readonly T[,] _array = new T[rows, columns];

    public int Height => _array.GetLength(0);

    public int Width => _array.GetLength(1);

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public T this[int row, int column]
    {
        get => _array[row, column];
        set => _array[row, column] = value;
    }

    public void SetRow(int row, T[] elements)
    {
        if (elements.Length != Width)
        {
            throw new InvalidOperationException("Element count and matrix column mismatch");
        }

        for (var i = 0; i < Width; i++)
        {
            _array[row, i] = elements[i];
        }
    }
}
