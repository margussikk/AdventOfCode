namespace AdventOfCode.Utilities.Geometry;

internal readonly struct GridCoordinate(int row, int column)
{
    public readonly int Row { get; } = row;

    public readonly int Column { get; } = column;

    public GridDirection RelativeDirection(GridCoordinate other)
    {
        if (Row == other.Row)
        {
            return Column > other.Column ? GridDirection.Right : GridDirection.Left;
        }
        else
        {
            return Row > other.Row ? GridDirection.Down : GridDirection.Up;
        }
    }

    public GridCoordinate MoveTo(GridDirection direction, int steps = 1)
    {
        return direction switch
        {
            GridDirection.Up => new GridCoordinate(Row - steps, Column),
            GridDirection.Down => new GridCoordinate(Row + steps, Column),
            GridDirection.Left => new GridCoordinate(Row, Column - steps),
            GridDirection.Right => new GridCoordinate(Row, Column + steps),
            GridDirection.None => this,
            _ => throw new InvalidOperationException("Unexpected direction")
        };
    }

    //public IEnumerable<GridCoordinate> Sides(GridDirection direction)
    //{
    //    if ((direction & GridDirection.Up) != GridDirection.None)
    //    {
    //        yield return new GridCoordinate(Row - 1, Column);
    //    }

    //    if ((direction & GridDirection.Down) != GridDirection.None)
    //    {
    //        yield return new GridCoordinate(Row + 1, Column);
    //    }

    //    if ((direction & GridDirection.Left) != GridDirection.None)
    //    {
    //        yield return new GridCoordinate(Row, Column - 1);
    //    }

    //    if ((direction & GridDirection.Right) != GridDirection.None)
    //    {
    //        yield return new GridCoordinate(Row, Column + 1);
    //    }
    //}

    public static bool operator ==(GridCoordinate coordinate1, GridCoordinate coordinate2)
    {
        return coordinate1.Equals(coordinate2);
    }

    public static bool operator !=(GridCoordinate coordinate1, GridCoordinate coordinate2)
    {
        return !coordinate1.Equals(coordinate2);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }

    public override bool Equals(object? obj)
    {
        return obj is GridCoordinate coordinate
            && Row == coordinate.Row
            && Column == coordinate.Column;
    }

    public override string ToString()
    {
        return $"{Row}, {Column}";
    }
}
