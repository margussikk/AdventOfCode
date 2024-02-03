namespace AdventOfCode.Utilities.Geometry;

internal readonly struct GridCell<T>(GridCoordinate coordinate, T obj)
{
    public GridCoordinate Coordinate { get; } = coordinate;
    public T Object { get; } = obj;
}
