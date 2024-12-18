namespace AdventOfCode.Utilities.Geometry;

internal readonly struct GridCoordinate(int row, int column) : IEquatable<GridCoordinate>
{
    public int Row { get; } = row;

    public int Column { get; } = column;

    public IEnumerable<GridCoordinate> UpNeighbors()
    {
        yield return new GridCoordinate(Row - 1, Column - 1);
        yield return new GridCoordinate(Row - 1, Column);
        yield return new GridCoordinate(Row - 1, Column + 1);
    }

    public IEnumerable<GridCoordinate> DownNeighbors()
    {
        yield return new GridCoordinate(Row + 1, Column - 1);
        yield return new GridCoordinate(Row + 1, Column);
        yield return new GridCoordinate(Row + 1, Column + 1);
    }

    public IEnumerable<GridCoordinate> LeftNeighbors()
    {
        yield return new GridCoordinate(Row - 1, Column - 1);
        yield return new GridCoordinate(Row, Column - 1);
        yield return new GridCoordinate(Row + 1, Column - 1);
    }

    public IEnumerable<GridCoordinate> RightNeighbors()
    {
        yield return new GridCoordinate(Row - 1, Column + 1);
        yield return new GridCoordinate(Row, Column + 1);
        yield return new GridCoordinate(Row + 1, Column + 1);
    }

    public IEnumerable<GridCoordinate> AroundNeighbors()
    {
        for (var drow = -1; drow <= 1; drow++)
        {
            for (var dcolumn = -1; dcolumn <= 1; dcolumn++)
            {
                if (drow != 0 || dcolumn != 0)
                {
                    yield return new GridCoordinate(Row + drow, Column + dcolumn);
                }
            }
        }
    }

    public IEnumerable<GridCoordinate> SideNeighbors()
    {
        yield return new GridCoordinate(Row - 1, Column);
        yield return new GridCoordinate(Row + 1, Column);
        yield return new GridCoordinate(Row, Column - 1);
        yield return new GridCoordinate(Row, Column + 1);
    }

    public GridDirection DirectionToward(GridCoordinate other)
    {
        var direction = GridDirection.None;

        if (other.Column > Column)
        {
            direction |= GridDirection.Right;
        }
        else if (other.Column < Column)
        {
            direction |= GridDirection.Left;
        }

        if (other.Row < Row)
        {
            direction |= GridDirection.Up;
        }
        else if (other.Row > Row)
        {
            direction |= GridDirection.Down;
        }

        return direction;
    }

    public GridCoordinate Move(GridDirection direction, int steps = 1)
    {
        return direction switch
        {
            GridDirection.Up => new GridCoordinate(Row - steps, Column),
            GridDirection.Down => new GridCoordinate(Row + steps, Column),
            GridDirection.Left => new GridCoordinate(Row, Column - steps),
            GridDirection.Right => new GridCoordinate(Row, Column + steps),

            GridDirection.UpLeft => new GridCoordinate(Row - steps, Column - steps),
            GridDirection.UpRight => new GridCoordinate(Row - steps, Column + steps),
            GridDirection.DownLeft => new GridCoordinate(Row + steps, Column - steps),
            GridDirection.DownRight => new GridCoordinate(Row + steps, Column + steps),

            GridDirection.None => this,
            _ => throw new InvalidOperationException($"Unexpected direction: {direction}")
        };
    }

    public GridCoordinate RotateClockwise()
    {
        return new GridCoordinate(Column, -Row);
    }

    public GridCoordinate RotateCounterClockwise()
    {
        return new GridCoordinate(-Column, Row);
    }

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

    public bool Equals(GridCoordinate other)
    {
        return Row == other.Row && Column == other.Column;
    }

    public override bool Equals(object? obj)
    {
        return obj is GridCoordinate coordinate && Equals(coordinate);
    }

    public override string ToString()
    {
        return $"{Row}, {Column}";
    }

    public static GridCoordinate Parse(string input)
    {
        var splits = input.Split(',');

        return new GridCoordinate(int.Parse(splits[1]), int.Parse(splits[0]));
    }
}
