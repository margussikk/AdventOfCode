using System.Collections;

namespace AdventOfCode.Utilities.Geometry;

// Axis aligned bounding box 3D
internal class Aabb3D: IEnumerable<Aabb3DCell>
{
    private readonly BitArray _bitArray;

    public Coordinate3D MinCoordinate { get; }

    public Coordinate3D MaxCoordinate { get; }

    public long XLength { get; }

    public long YLength { get; }

    public long ZLength { get; }

    public Aabb3D(Coordinate3D minCoordinate, Coordinate3D maxCoordinate)
    {
        MinCoordinate = minCoordinate;
        MaxCoordinate = maxCoordinate;

        XLength = MaxCoordinate.X - MinCoordinate.X + 1;
        YLength = MaxCoordinate.Y - MinCoordinate.Y + 1;
        ZLength = MaxCoordinate.Z - MinCoordinate.Z + 1;

        var length = XLength * YLength * ZLength;
        _bitArray = new BitArray((int)length);
    }

    public bool this[long x, long y, long z]
    {
        get => _bitArray.Get(GetIndex(x, y, z));
        set => _bitArray.Set(GetIndex(x, y, z), value);
    }

    public bool this[Coordinate3D coordinate]
    {
        get => this[coordinate.X, coordinate.Y, coordinate.Z];
        set => this[coordinate.X, coordinate.Y, coordinate.Z] = value;
    }

    public bool InBounds(Coordinate3D coordinate)
    {
        return coordinate.X >= MinCoordinate.X && coordinate.X <= MaxCoordinate.X &&
               coordinate.Y >= MinCoordinate.Y && coordinate.Y <= MaxCoordinate.Y &&
               coordinate.Z >= MinCoordinate.Z && coordinate.Z <= MaxCoordinate.Z;
    }

    public IEnumerable<Aabb3DCell> AroundNeighbors(Coordinate3D coordinate)
    {
        foreach (var neighborCoordinate in coordinate.AroundNeighbors().Where(InBounds))
        {
            yield return new Aabb3DCell(neighborCoordinate, this[neighborCoordinate]);
        }
    }

    public IEnumerable<Aabb3DCell> Window(Coordinate3D minCoordinate, Coordinate3D maxCoordinate)
    {
        for (var z = minCoordinate.Z; z <= maxCoordinate.Z; z++)
        {
            for (var y = minCoordinate.Y; y <= maxCoordinate.Y; y++)
            {
                for (var x = minCoordinate.X; x <= maxCoordinate.X; x++)
                {
                    var coordinate = new Coordinate3D(x, y, z);
                    yield return new Aabb3DCell(coordinate, this[coordinate]);
                }
            }
        }
    }

    public IEnumerator<Aabb3DCell> GetEnumerator()
    {
        return Window(MinCoordinate, MaxCoordinate).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private int GetIndex(long x, long y, long z)
    {
        var index = (x - MinCoordinate.X) * YLength * ZLength + (y - MinCoordinate.Y) * ZLength + (z - MinCoordinate.Z);
        return (int)index;
    }
}