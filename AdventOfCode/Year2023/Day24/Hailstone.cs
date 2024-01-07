namespace AdventOfCode.Year2023.Day24;

internal class Hailstone(long x, long y, long z, long dX, long dY, long dZ)
{
    public long X { get; } = x;
    public long Y { get; } = y;
    public long Z { get; } = z;
    public long DX { get; } = dX;
    public long DY { get; } = dY;
    public long DZ { get; } = dZ;

    public static Hailstone Parse(string input)
    {
        var splits = input.Split('@');

        var positionCoordinates = splits[0]
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToList();

        var velocityCoordinates = splits[1]
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToList();

        return new Hailstone(positionCoordinates[0], positionCoordinates[1], positionCoordinates[2],
                             velocityCoordinates[0], velocityCoordinates[1], velocityCoordinates[2]);
    }
}
