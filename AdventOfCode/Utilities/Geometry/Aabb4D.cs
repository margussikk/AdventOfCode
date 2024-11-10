using System.Collections;

namespace AdventOfCode.Utilities.Geometry;

// Axis aligned bounding box 4D
internal class Aabb4D: IEnumerable<Aabb4DCell>
{
    private readonly BitArray _bitArray;

    public Coordinate4D MinCoordinate { get; }

    public Coordinate4D MaxCoordinate { get; }

    public long XLength { get; }

    public long YLength { get; }

    public long ZLength { get; }

    public long WLength { get; }

    public Aabb4D(Coordinate4D minCoordinate, Coordinate4D maxCoordinate)
    {
        MinCoordinate = minCoordinate;
        MaxCoordinate = maxCoordinate;

        XLength = MaxCoordinate.X - MinCoordinate.X + 1;
        YLength = MaxCoordinate.Y - MinCoordinate.Y + 1;
        ZLength = MaxCoordinate.Z - MinCoordinate.Z + 1;
        WLength = MaxCoordinate.W - MinCoordinate.W + 1;

        var length = XLength * YLength * ZLength * WLength;
        _bitArray = new BitArray((int)length);
    }

    public bool this[long x, long y, long z, long w]
    {
        get => _bitArray.Get(GetIndex(x, y, z, w));
        set => _bitArray.Set(GetIndex(x, y, z, w), value);
    }

    public bool this[Coordinate4D coordinate]
    {
        get => this[coordinate.X, coordinate.Y, coordinate.Z, coordinate.W];
        set => this[coordinate.X, coordinate.Y, coordinate.Z, coordinate.W] = value;
    }

    public bool InBounds(Coordinate4D coordinate)
    {
        return coordinate.X >= MinCoordinate.X && coordinate.X <= MaxCoordinate.X &&
               coordinate.Y >= MinCoordinate.Y && coordinate.Y <= MaxCoordinate.Y &&
               coordinate.Z >= MinCoordinate.Z && coordinate.Z <= MaxCoordinate.Z &&
               coordinate.W >= MinCoordinate.W && coordinate.W <= MaxCoordinate.W;
    }

    public IEnumerable<Aabb4DCell> AroundNeighbors(Coordinate4D coordinate)
    {
        foreach (var neighborCoordinate in coordinate.AroundNeighbors().Where(InBounds))
        {
            yield return new Aabb4DCell(neighborCoordinate,  this[neighborCoordinate]);
        }
    }

    public IEnumerable<Aabb4DCell> Window(Coordinate4D minCoordinate, Coordinate4D maxCoordinate)
    {
        for (var w = minCoordinate.W; w <= maxCoordinate.W; w++)
        {
            for (var z = minCoordinate.Z; z <= maxCoordinate.Z; z++)
            {
                for (var y = minCoordinate.Y; y <= maxCoordinate.Y; y++)
                {
                    for (var x = minCoordinate.X; x <= maxCoordinate.X; x++)
                    {
                        var coordinate = new Coordinate4D(x, y, z, w);
                        yield return new Aabb4DCell(coordinate, this[coordinate]);
                    }
                }
            }
        }
    }

    public IEnumerator<Aabb4DCell> GetEnumerator()
    {
        return Window(MinCoordinate, MaxCoordinate).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private int GetIndex(long x, long y, long z, long w)
    {
        var index = (x - MinCoordinate.X) * YLength * ZLength * WLength +
            (y - MinCoordinate.Y) * ZLength * WLength +
            (z - MinCoordinate.Z) * WLength +
            (w - MinCoordinate.W);
        return (int)index;
    }
}