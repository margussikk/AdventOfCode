namespace AdventOfCode.Utilities.Geometry;

internal class Aabb3DCell(Coordinate3D coordinate, bool active)
{
    public Coordinate3D Coordinate { get; } = coordinate;
    public bool Active { get; } = active;
}
