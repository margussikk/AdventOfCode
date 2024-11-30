using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day22;

internal class Brick
{
    public int Id { get; private init; } // Good for debugging
    public Coordinate3D Start { get; private set; }
    public Coordinate3D End { get; private set; }

    public HashSet<Brick> Supports { get; private set; } = [];

    public HashSet<Brick> SupportedBy { get; private set; } = [];

    public Brick CleanClone()
    {
        var clone = new Brick
        {
            Id = Id,
            Start = Start,
            End = End
        };

        return clone;
    }

    public void DropToZ(long z)
    {
        if (Start.Z == z) return;

        // NB! Set End before Start, because End uses Start
        End = new Coordinate3D(End.X, End.Y, End.Z - Start.Z + z);
        Start = new Coordinate3D(Start.X, Start.Y, z);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id.GetHashCode(), Start.GetHashCode(), End.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        return obj is Brick other &&
            Id == other.Id &&
            Start == other.Start &&
            End == other.End; // Risky, but ignore Supports and SupportedBy
    }

    public static Brick Parse(int id, string input)
    {
        var coordinates = input.Split('~')
                               .Select(Coordinate3D.Parse)
                               .ToArray();

        // Adjust the block such that Start coordinates are less than End coordinates
        if (coordinates[0].X != coordinates[1].X)
        {
            coordinates = [.. coordinates.OrderBy(x => x.X)];
        }
        else if (coordinates[0].Y != coordinates[1].Y)
        {
            coordinates = [.. coordinates.OrderBy(x => x.Y)];
        }
        else
        {
            coordinates = [.. coordinates.OrderBy(x => x.Z)];
        }

        return new Brick
        {
            Id = id,
            Start = coordinates[0],
            End = coordinates[1]
        };
    }

    public static Brick CreateGroundBrick(long maxX, long maxY)
    {
        return new Brick
        {
            Id = 0,
            Start = new Coordinate3D(0, 0, 0),
            End = new Coordinate3D(maxX, maxY, 0)
        };
    }
}
