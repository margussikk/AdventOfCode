namespace AdventOfCode.Utilities.GridSystem;

internal class HexGridCell<T>(HexCoordinate coordinate, T obj)
{
    public HexCoordinate Coordinate { get; } = coordinate;
    public T Object { get; } = obj;
}