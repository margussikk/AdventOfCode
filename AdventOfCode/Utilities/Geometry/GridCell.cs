namespace AdventOfCode.Utilities.Geometry;

internal readonly struct GridCell<T>(T obj, GridCoordinate coordinate)
{
    public T Object { get; } = obj;

    public GridCoordinate Coordinate { get; } = coordinate;
}
