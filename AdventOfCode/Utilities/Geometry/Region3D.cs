namespace AdventOfCode.Utilities.Geometry;

internal class Region3D
{
    public Coordinate3D MinCoordinate { get; }

    public Coordinate3D MaxCoordinate { get; }

    public Coordinate3D CenterCoordinate => new((MinCoordinate.X + MaxCoordinate.X) / 2,
                                                (MinCoordinate.Y + MaxCoordinate.Y) / 2,
                                                (MinCoordinate.Z + MaxCoordinate.Z) / 2);
    public long XLength { get; }

    public long YLength { get; }

    public long ZLength { get; }

    public long Volume => XLength * YLength * ZLength;

    public Region3D(IEnumerable<Coordinate3D> coordinates)
    {
        var minX = long.MaxValue;
        var maxX = long.MinValue;

        var minY = long.MaxValue;
        var maxY = long.MinValue;

        var minZ = long.MaxValue;
        var maxZ = long.MinValue;

        foreach (var coordinate in coordinates)
        {
            minX = long.Min(minX, coordinate.X);
            maxX = long.Max(maxX, coordinate.X);

            minY = long.Min(minY, coordinate.Y);
            maxY = long.Max(maxY, coordinate.Y);

            minZ = long.Min(minZ, coordinate.Z);
            maxZ = long.Max(maxZ, coordinate.Z);
        }

        MinCoordinate = new Coordinate3D(minX, minY, minZ);
        MaxCoordinate = new Coordinate3D(maxX, maxY, maxZ);

        XLength = MaxCoordinate.X - MinCoordinate.X + 1;
        YLength = MaxCoordinate.Y - MinCoordinate.Y + 1;
        ZLength = MaxCoordinate.Z - MinCoordinate.Z + 1;
    }
    
    public Region3D(Coordinate3D minCoordinate, Coordinate3D maxCoordinate)
    {
        MinCoordinate = minCoordinate;
        MaxCoordinate = maxCoordinate;

        XLength = MaxCoordinate.X - MinCoordinate.X + 1;
        YLength = MaxCoordinate.Y - MinCoordinate.Y + 1;
        ZLength = MaxCoordinate.Z - MinCoordinate.Z + 1;
    }

    public bool InBounds(Coordinate3D coordinate)
    {
        return coordinate.X >= MinCoordinate.X &&
               coordinate.X <= MaxCoordinate.X &&
               coordinate.Y >= MinCoordinate.Y &&
               coordinate.Y <= MaxCoordinate.Y &&
               coordinate.Z >= MinCoordinate.Z &&
               coordinate.Z <= MaxCoordinate.Z;
    }

    public long Distance(Coordinate3D coordinate)
    {
        // Region.InBounds is checked using Math.Max(0, ...)
        var dx = Math.Max(0, Math.Max(MinCoordinate.X - coordinate.X, coordinate.X - MaxCoordinate.X));
        var dy = Math.Max(0, Math.Max(MinCoordinate.Y - coordinate.Y, coordinate.Y - MaxCoordinate.Y));
        var dz = Math.Max(0, Math.Max(MinCoordinate.Z - coordinate.Z, coordinate.Z - MaxCoordinate.Z));

        return dx + dy + dz;
    }

    public IEnumerable<Region3D> Divide()
    {
        if (Volume == 1)
        {
            yield return this;
        }

        var x1 = MinCoordinate.X + (XLength - 1) / 2;
        var x2 = x1 + 1;
        var y1 = MinCoordinate.Y + (YLength - 1) / 2;
        var y2 = y1 + 1;
        var z1 = MinCoordinate.Z + (ZLength - 1) / 2;
        var z2 = z1 + 1;

        yield return new Region3D(new Coordinate3D(MinCoordinate.X, MinCoordinate.Y, MinCoordinate.Z), new Coordinate3D(x1, y1, z1));

        if (XLength > 1)
        {
            yield return new Region3D(new Coordinate3D(x2, MinCoordinate.Y, MinCoordinate.Z), new Coordinate3D(MaxCoordinate.X, y1, z1));
        }

        if (YLength > 1)
        {
            yield return new Region3D(new Coordinate3D(MinCoordinate.X, y2, MinCoordinate.Z), new Coordinate3D(x1, MaxCoordinate.Y, z1));
        }

        if (XLength > 1 && YLength > 1)
        {
            yield return new Region3D(new Coordinate3D(x2, y2, MinCoordinate.Z), new Coordinate3D(MaxCoordinate.X, MaxCoordinate.Y, z1));
        }

        if (ZLength > 1)
        {
            yield return new Region3D(new Coordinate3D(MinCoordinate.X, MinCoordinate.Y, z2), new Coordinate3D(x1, y1, MaxCoordinate.Z));
        }

        if (XLength > 1 && ZLength > 1)
        {
            yield return new Region3D(new Coordinate3D(x2, MinCoordinate.Y, z2), new Coordinate3D(MaxCoordinate.X, y1, MaxCoordinate.Z));
        }

        if (YLength > 1 && ZLength > 1)
        {
            yield return new Region3D(new Coordinate3D(MinCoordinate.X, y2, z2), new Coordinate3D(x1, MaxCoordinate.Y, MaxCoordinate.Z));
        }

        if (XLength > 1 && YLength > 1 && ZLength > 1)
        {
            yield return new Region3D(new Coordinate3D(x2, y2, z2), new Coordinate3D(MaxCoordinate.X, MaxCoordinate.Y, MaxCoordinate.Z));
        }
    }
}
