using BenchmarkDotNet.Columns;
using System.Collections;
using System.Text;

namespace AdventOfCode.Utilities.GridSystem;

internal abstract class GridBase<TObject> : IEnumerable<GridCell<TObject>>
{
    public int Width => LastColumn - FirstColumn + 1;
    public int Height => LastRow - FirstRow + 1;

    public int FirstRow { get; protected set; }
    public int LastRow { get; protected set; }

    public int FirstColumn { get; protected set; }
    public int LastColumn { get; protected set; }

    public virtual bool InBounds(int row, int column)
    {
        return row >= FirstRow && row <= LastRow &&
               column >= FirstColumn && column <= LastColumn;
    }

    public virtual bool InBounds(GridCoordinate coordinate)
    {
        return InBounds(coordinate.Row, coordinate.Column);
    }

    public TObject this[GridCoordinate coordinate]
    {
        get => this[coordinate.Row, coordinate.Column];
        set => this[coordinate.Row, coordinate.Column] = value;
    }

    public abstract TObject this[int row, int column] { get; set; }

    public GridCell<TObject> Cell(GridCoordinate coordinate)
    {
        return new GridCell<TObject>(coordinate, this[coordinate.Row, coordinate.Column]);
    }

    public IEnumerable<GridCell<TObject>> Row(int row)
    {
        for (var column = FirstColumn; column <= LastColumn; column++)
        {
            yield return new GridCell<TObject>(new GridCoordinate(row, column), this[row, column]);
        }
    }

    public IEnumerable<GridCell<TObject>> Column(int column)
    {
        for (var row = FirstRow; row <= LastRow; row++)
        {
            yield return new GridCell<TObject>(new GridCoordinate(row, column), this[row, column]);
        }
    }

    public IEnumerable<GridCell<TObject>> SideNeighbors(GridCoordinate coordinate)
    {
        foreach (var neighborCoordinate in coordinate.SideNeighbors().Where(InBounds))
        {
            yield return new GridCell<TObject>(neighborCoordinate, this[neighborCoordinate]);
        }
    }

    public IEnumerable<GridCell<TObject>> AroundNeighbors(GridCoordinate coordinate)
    {
        foreach (var neighborCoordinate in coordinate.AroundNeighbors().Where(InBounds))
        {
            yield return new GridCell<TObject>(neighborCoordinate, this[neighborCoordinate]);
        }
    }

    public GridCoordinate? FindCoordinate(Func<TObject, bool> predicate)
    {
        for (var row = FirstRow; row <= LastRow; row++)
        {
            for (var column = FirstColumn; column <= LastColumn; column++)
            {
                if (predicate(this[row, column]))
                {
                    return new GridCoordinate(row, column);
                }
            }
        }

        return null;
    }

    protected TGrid RotateClockwise<TGrid>(TGrid grid) where TGrid : GridBase<TObject>
    {
        for (var row = FirstRow; row <= grid.LastRow; row++)
        {
            for (var column = FirstColumn; column <= grid.LastColumn; column++)
            {
                grid[row, column] = this[grid.LastColumn - column, row];
            }
        }

        return grid;
    }

    protected TGrid RotateCounterClockwise<TGrid>(TGrid grid) where TGrid : GridBase<TObject>
    {
        for (var row = FirstRow; row <= grid.LastRow; row++)
        {
            for (var column = FirstColumn; column <= grid.LastColumn; column++)
            {
                grid[row, column] = this[column, grid.LastRow - row];
            }
        }

        return grid;
    }

    protected TGrid FlipHorizontally<TGrid>(TGrid grid) where TGrid : GridBase<TObject>
    {
        for (var row = grid.FirstRow; row <= grid.LastRow; row++)
        {
            for (var column = grid.FirstColumn; column <= grid.LastColumn; column++)
            {
                grid[row, column] = this[row, grid.LastColumn - column];
            }
        }

        return grid;
    }

    protected TGrid FlipVertically<TGrid>(TGrid grid) where TGrid : GridBase<TObject>
    {
        for (var row = grid.FirstRow; row <= grid.LastRow; row++)
        {
            for (var column = grid.FirstColumn; column <= grid.LastColumn; column++)
            {
                grid[row, column] = this[grid.LastRow - row, column];
            }
        }

        return grid;
    }

    public WindowGrid<TObject> Window(GridCoordinate topLeftCoordinate, GridCoordinate bottomRightCoordinate)
    {
        return new WindowGrid<TObject>(this, topLeftCoordinate, bottomRightCoordinate);
    }

    public void CopyFrom(GridBase<TObject> grid, GridCoordinate topLeftCoordinate)
    {
        for (var row = grid.FirstRow; row <= grid.LastRow; row++)
        {
            for (var column = grid.FirstColumn; column <= grid.LastColumn; column++)
            {
                this[topLeftCoordinate.Row + row, topLeftCoordinate.Column + column] = grid[row, column];
            }
        }
    }

    public IEnumerator<GridCell<TObject>> GetEnumerator()
    {
        for (var row = FirstRow; row <= LastRow; row++)
        {
            for (var column = FirstColumn; column <= LastColumn; column++)
            {
                yield return new GridCell<TObject>(new GridCoordinate(row, column), this[row, column]);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Print(Func<TObject, char> mapper)
    {
        Console.WriteLine(PrintToString(mapper));
    }

    public void PrintToFile(string path, Func<TObject, char> mapper)
    {
        File.WriteAllText(path, PrintToString(mapper));
    }

    public string PrintToString(Func<TObject, char> mapper)
    {
        var stringBuilder = new StringBuilder();

        for (var row = FirstRow; row <= LastRow; row++)
        {
            for (var column = FirstColumn; column <= LastColumn; column++)
            {
                stringBuilder.Append(mapper(this[row, column]));
            }
            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }
}
