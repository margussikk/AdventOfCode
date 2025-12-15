using AdventOfCode.Utilities.Mathematics;
using Spectre.Console;

namespace AdventOfCode.Utilities.Numerics;

internal class Matrix2
{
    private readonly RationalNumber[,] _array;

    public int Height => _array.GetLength(0);

    public int Width => _array.GetLength(1);

    public int LastRowIndex => Height - 1;

    public int LastColumnIndex => Width - 1;

    public Matrix2(int rows, int columns)
    {
        _array = new RationalNumber[rows, columns];
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                _array[row, column] = RationalNumber.Zero;
            }
        }
    }

    public RationalNumber this[int row, int column]
    {
        get => _array[row, column];
        set => _array[row, column] = value;
    }

    public void TransformToReducedRowEchelonForm()
    {
        // Gauss-Jordan Elimination 
        var currentColumn = 0;
        for (var currentRow = 0; currentRow <= LastRowIndex; currentRow++)
        {
            for (; currentColumn < LastColumnIndex; currentColumn++)
            {
                // Swap rows
                var swapRow = -1;

                for (var row = currentRow; row <= LastRowIndex; row++)
                {
                    if (this[row, currentColumn] != RationalNumber.Zero)
                    {
                        swapRow = row;
                        break;
                    }
                }

                if (swapRow < 0)
                {
                    continue;
                }
                else if (swapRow != currentRow)
                {
                    SwapRows(currentRow, swapRow);
                }

                // Make pivot an unit
                if (this[currentRow, currentColumn] != RationalNumber.One)
                {
                    var divider = this[currentRow, currentColumn];
                    DivideRow(currentRow, divider);
                }

                // Make rows above and below 0
                for (var row = 0; row <= LastRowIndex; row++)
                {
                    if (row == currentRow)
                    {
                        continue;
                    }

                    var multiplier = RationalNumber.Zero - this[row, currentColumn];
                    AddAnotherRow(row, currentRow, multiplier);
                }

                currentColumn++;
                break;
            }
        }
    }

    public IReadOnlyList<LinearEquation> GetLinearEquations()
    {
        var equations = Enumerable.Range(0, Width - 1)
            .Select(x => new LinearEquation(x))
            .ToArray();

        for (var row = 0; row <= LastRowIndex; row++)
        {
            var variableColumn = FindNonZeroColumnIndex(row);
            if (variableColumn < 0 || variableColumn == LastColumnIndex)
            {
                continue;
            }
            else if (this[row, variableColumn] != RationalNumber.One)
            {
                throw new InvalidOperationException("Variable element should be one");
            }

            var startColumn = variableColumn + 1;
            var linearTerms = Enumerable
                .Range(startColumn, Width - startColumn)
                .Where(column => this[row, column] != RationalNumber.Zero)
                .Select(column => column == LastColumnIndex
                    ? new LinearTerm
                    {
                        Coefficient = this[row, column]
                    }
                    :
                    new LinearTerm
                    {
                        Coefficient = RationalNumber.Zero - this[row, column],
                        Variable = column
                    })
                .ToList();
            var linearExpression = new LinearExpression(linearTerms);

            equations[variableColumn] = new LinearEquation(variableColumn, linearExpression);
        }

        return equations;
    }

    private int FindNonZeroColumnIndex(int row)
    {
        for (var column = 0; column <= LastColumnIndex; column++)
        {
            if (this[row, column] != RationalNumber.Zero)
            {
                return column;
            }
        }

        return -1;
    }

    public void SwapRows(int row1, int row2)
    {
        for (var column = 0; column <= LastColumnIndex; column++)
        {
            (this[row1, column], this[row2, column]) = (this[row2, column], this[row1, column]);
        }
    }

    public void DivideRow(int row, RationalNumber divider)
    {
        for (var column = 0; column <= LastColumnIndex; column++)
        {
            this[row, column] /= divider;
        }
    }

    public void AddAnotherRow(int currentRow, int anotherRow, RationalNumber anotherRowMultiplier)
    {
        if (anotherRowMultiplier == RationalNumber.Zero)
        {
            return;
        }

        for (var column = 0; column <= LastColumnIndex; column++)
        {
            this[currentRow, column] += anotherRowMultiplier * this[anotherRow, column];
        }
    }

    public void Print()
    {
        var totalWidth = 0;

        for (var row = 0; row <= LastRowIndex; row++)
        {
            for (var column = 0; column <= LastColumnIndex; column++)
            {
                totalWidth = Math.Max(totalWidth, this[row, column].ToString().Length + 2);
            }
        }

        for (var row = 0; row <= LastRowIndex; row++)
        {
            for (var column = 0; column <= LastColumnIndex; column++)
            {
                Console.Write(this[row, column].ToString().PadLeft(totalWidth));
            }

            Console.WriteLine();
        }
    }
}
