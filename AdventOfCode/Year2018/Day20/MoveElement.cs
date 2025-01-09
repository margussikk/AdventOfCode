using AdventOfCode.Utilities.GridSystem;
using System.Text;

namespace AdventOfCode.Year2018.Day20;
internal class MoveElement : RouteElement
{
    public List<GridDirection> Directions { get; private set; } = [];

    public static MoveElement Parse(ref ReadOnlySpan<char> span)
    {
        var sequence = new MoveElement();

        while (span.Length > 0 && span[0] is 'W' or 'N' or 'E' or 'S')
        {
            var direction = span[0] switch
            {
                'W' => GridDirection.Left,
                'N' => GridDirection.Up,
                'E' => GridDirection.Right,
                'S' => GridDirection.Down,
                _ => throw new InvalidOperationException($"Invalid direction characted: {span[0]}")
            };

            sequence.Directions.Add(direction);
            span = span[1..];
        }

        return sequence;
    }

    public override GridCoordinate Walk(GridCoordinate coordinate, InfiniteGrid<bool> grid)
    {
        foreach (var direction in Directions)
        {
            coordinate = coordinate.Move(direction);
            grid[coordinate] = true;

            coordinate = coordinate.Move(direction);
            grid[coordinate] = true;
        }

        return coordinate;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        foreach (var direction in Directions)
        {
            var character = direction switch
            {
                GridDirection.Left => 'W',
                GridDirection.Up => 'N',
                GridDirection.Right => 'E',
                GridDirection.Down => 'S',
                _ => throw new InvalidOperationException($"Invalid direction: {direction}")
            };

            stringBuilder.Append(character);
        }

        return stringBuilder.ToString();
    }
}
