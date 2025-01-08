namespace AdventOfCode.Utilities.GridSystem;
internal class Grid<TObject> : GridBase<TObject>
{
    private readonly TObject[,] _array;

    public Grid(int height, int width)
    {
        _array = new TObject[height, width];

        FirstRow = 0;
        LastRow = height - 1;

        FirstColumn = 0;
        LastColumn = width - 1;
    }

    public Grid<TObject> Clone()
    {
        var grid = new Grid<TObject>(Height, Width);

        Array.Copy(_array, grid._array, _array.Length);

        return grid;
    }

    public override bool Equals(object? obj)
    {
        return obj is Grid<TObject> grid &&
               Height == grid.Height &&
               Width == grid.Width &&
               EqualityComparer<TObject[,]>.Default.Equals(_array, grid._array);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var item in _array)
        {
            hashCode.Add(item!.GetHashCode());
        }

        return hashCode.ToHashCode();
    }

    public override TObject this[int row, int column]
    {
        get => _array[row, column];
        set => _array[row, column] = value;
    }

    public Grid<TObject> RotateClockwise() => RotateClockwise(new Grid<TObject>(Width, Height));

    public Grid<TObject> RotateCounterClockwise() => RotateCounterClockwise(new Grid<TObject>(Width, Height));

    public Grid<TObject> FlipHorizontally() => FlipHorizontally(new Grid<TObject>(Height, Width));

    public Grid<TObject> FlipVertically() => FlipVertically(new Grid<TObject>(Height, Width));

}
