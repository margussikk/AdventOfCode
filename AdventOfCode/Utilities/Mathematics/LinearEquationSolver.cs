using AdventOfCode.Utilities.Numerics;
using System.Numerics;

namespace AdventOfCode.Utilities.Mathematics;

internal static class LinearEquationSolver
{
    // Function to get matrix content
    public static bool TrySolveLinearEquation<T>(Matrix<T> matrix, out T[] values) where T : INumber<T>
    {
        if (matrix.Height != matrix.Width - 1)
        {
            // Invalid matrix size
            values = [];
            return false;
        }

        var singularFlag = ForwardSubstitution(matrix);
        if (singularFlag != -1)
        {
            // If singularFlag == 0 then there are infinitely many solutions.
            // If singularFlar > 0 then the system is inconsistent

            values = [];
            return false;
        }

        values = BackSubstitute(matrix);
        return true;
    }

    // Function to reduce matrix to row echelon form.
    private static int ForwardSubstitution<T>(Matrix<T> matrix) where T : INumber<T>
    {
        for (var currentRow = 0; currentRow < matrix.Height; currentRow++)
        {
            var currentColumn = currentRow; // Use separate variables for clarity

            // Find pivot row
            var pivotRow = currentRow;
            for (var row = currentRow + 1; row < matrix.Height; row++)
            {
                if (T.Abs(matrix[row, currentColumn]) > T.Abs(matrix[pivotRow, currentColumn]))
                {
                    pivotRow = row;
                }
            }

            // Make sure the current row is the pivot row.
            if (currentRow != pivotRow)
            {
                for (var column = 0; column < matrix.Width; column++)
                {
                    (matrix[currentRow, column], matrix[pivotRow, column]) = (matrix[pivotRow, column], matrix[currentRow, column]);
                }
            }

            // If a principal diagonal element  is zero, it denotes that the
            // matrix is singular, and it will lead to a division-by-zero later.
            if (matrix[currentRow, currentColumn] == T.Zero)
            {
                return currentColumn; // Matrix is singular
            }

            for (var row = currentRow + 1; row < matrix.Height; row++)
            {
                var factor = matrix[row, currentColumn] / matrix[currentRow, currentColumn];

                for (var column = currentColumn + 1; column < matrix.Width; column++)
                {
                    matrix[row, column] -= matrix[currentRow, column] * factor;
                }

                matrix[row, currentColumn] = T.Zero;
            }
        }

        return -1;
    }

    // Function to calculate the values of the unknowns
    private static T[] BackSubstitute<T>(Matrix<T> matrix) where T : INumber<T>
    {
        // An array to store solution
        var values = new T[matrix.Height];

        // Start calculating from last equation up to the first
        for (var currentRow = matrix.LastRowIndex; currentRow >= 0; currentRow--)
        {
            var currentColumn = currentRow; // Use separate variables for clarity
            var value = matrix[currentRow, matrix.LastColumnIndex];

            for (var column = currentColumn + 1; column < matrix.LastColumnIndex; column++)
            {
                value -= matrix[currentRow, column] * values[column];
            }

            values[currentColumn] = value / matrix[currentRow, currentColumn];
        }

        return values;
    }
}
