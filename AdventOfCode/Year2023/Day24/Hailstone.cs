namespace AdventOfCode.Year2023.Day24;

internal class Hailstone
{
    public long X { get; }
    public long Y { get; }
    public long Z { get; }
    public long DX { get; }
    public long DY { get; }
    public long DZ { get; }

    public Hailstone(long x, long y, long z, long dX, long dY, long dZ)
    {
        X = x;
        Y = y;
        Z = z;
        DX = dX;
        DY = dY;
        DZ = dZ;
    }
    
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
