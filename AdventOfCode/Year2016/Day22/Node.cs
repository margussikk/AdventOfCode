using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2016.Day22;

internal class Node
{
    public GridCoordinate Coordinate { get; set; }

    public int Size { get; set; }

    public int Used { get; set; }

    public int Avail => Size - Used;

    public static Node Parse(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var coordinateParts = parts[0].Split('-');

        var node = new Node
        {
            Coordinate = new GridCoordinate(int.Parse(coordinateParts[2][1..]), int.Parse(coordinateParts[1][1..])),
            Size = int.Parse(parts[1][..^1]),
            Used = int.Parse(parts[2][..^1]),
        };

        return node;
    }
}
