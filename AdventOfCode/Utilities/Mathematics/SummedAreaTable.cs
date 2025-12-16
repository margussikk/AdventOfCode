namespace AdventOfCode.Utilities.Mathematics;

// https://en.wikipedia.org/wiki/Summed-area_table
internal class SummedAreaTable
{
    private readonly int[,] _array;

    public int Height => _array.GetLength(0);

    public int Width => _array.GetLength(1);

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public SummedAreaTable(int[,] matrix)
    {
        _array = new int[matrix.GetLength(0), matrix.GetLength(1)];

        // Create summed area table
        // I(x, y) = i(x, y) + I(x, y - 1) + I(x - 1, y) - I(x - 1, y - 1)
        for (var row = 0; row < _array.GetLength(0); row++)
        {
            for (var column = 0; column < _array.GetLength(1); column++)
            {
                var value = matrix[row, column];
                if (column > 0)
                {
                    value += _array[row, column - 1];
                }

                if (row > 0)
                {
                    value += _array[row - 1, column];
                }

                if (column > 0 && row > 0)
                {
                    value -= _array[row - 1, column - 1];
                }

                _array[row, column] = value;
            }
        }
    }

    public int this[int row, int column]
    {
        get => _array[row, column];
    }

    // i(x, y) = I(D) + I(A) - I(B) - I(C)
    public int GetRectangleSum(int topRow, int leftColumn, int bottomRow, int rightColumn)
    {
        var value = _array[bottomRow, rightColumn];

        if (topRow >= 1 && leftColumn >= 1)
        {
            value += _array[topRow - 1, leftColumn - 1];
        }

        if (topRow >= 1)
        {
            value -= _array[topRow - 1, rightColumn];
        }

        if (leftColumn >= 1)
        {
            value -= _array[bottomRow, leftColumn - 1];
        }

        return value;
    }
}
