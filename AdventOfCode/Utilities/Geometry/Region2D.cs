using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Utilities.Geometry;

internal class Region2D : IEnumerable<Coordinate2D>
{
    public Coordinate2D MinCoordinate { get; }

    public Coordinate2D MaxCoordinate { get; }

    public long XLength { get; }

    public long YLength { get; }

    public Region2D(IEnumerable<Coordinate2D> coordinates)
    {
        var minX = long.MaxValue;
        var maxX = long.MinValue;

        var minY = long.MaxValue;
        var maxY = long.MinValue;

        foreach (var coordinate in coordinates)
        {
            minX = long.Min(minX, coordinate.X);
            maxX = long.Max(maxX, coordinate.X);

            minY = long.Min(minY, coordinate.Y);
            maxY = long.Max(maxY, coordinate.Y);
        }

        MinCoordinate = new Coordinate2D(minX, minY);
        MaxCoordinate = new Coordinate2D(maxX, maxY);

        XLength = MaxCoordinate.X - MinCoordinate.X + 1;
        YLength = MaxCoordinate.Y - MinCoordinate.Y + 1;
    }

    public Region2D(Coordinate2D minCoordinate, Coordinate2D maxCoordinate)
    {
        MinCoordinate = minCoordinate;
        MaxCoordinate = maxCoordinate;

        XLength = MaxCoordinate.X - MinCoordinate.X + 1;
        YLength = MaxCoordinate.Y - MinCoordinate.Y + 1;
    }

    public bool InBounds(Coordinate2D coordinate)
    {
        return coordinate.X >= MinCoordinate.X &&
               coordinate.X <= MaxCoordinate.X &&
               coordinate.Y >= MinCoordinate.Y &&
               coordinate.Y <= MaxCoordinate.Y;
    }

    public bool TryFindOverlap(Region2D other, [MaybeNullWhen(false)] out Region2D overlapRegion)
    {
        if (Overlaps(other))
        {
            var minCoordinate = new Coordinate2D(Math.Max(MinCoordinate.X, other.MinCoordinate.X), Math.Max(MinCoordinate.Y, other.MinCoordinate.Y));
            var maxCoordinate = new Coordinate2D(Math.Min(MaxCoordinate.X, other.MaxCoordinate.X), Math.Min(MaxCoordinate.Y, other.MaxCoordinate.Y));

            overlapRegion = new Region2D(minCoordinate, maxCoordinate);

            return true;
        }

        overlapRegion = null;
        return false;
    }

    public bool Overlaps(Region2D other)
    {
        return MinCoordinate.X <= other.MaxCoordinate.X &&
               MaxCoordinate.X >= other.MinCoordinate.X &&
               MinCoordinate.Y <= other.MaxCoordinate.Y &&
               MaxCoordinate.Y >= other.MinCoordinate.Y;
    }

    public IEnumerator<Coordinate2D> GetEnumerator()
    {
        for (var y = MinCoordinate.Y; y <= MaxCoordinate.Y; y++)
        {
            for (var x = MinCoordinate.X; x <= MaxCoordinate.X; x++)
            {
                yield return new Coordinate2D(x, y);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
