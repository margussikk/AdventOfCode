using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2018.Day20;
internal class RootElement : RouteElement
{
    public static RootElement Parse(string input)
    {
        var root = new RootElement();

        var span = input.AsSpan();

        if (span[0] != '^')
        {
            throw new InvalidOperationException("Regex should start with ^");
        }

        span = span[1..]; // Skip ^

        var container = ContainerElement.Parse(ref span);
        root.Elements.Add(container);

        if (span[0] != '$')
        {
            throw new InvalidOperationException("Regex should end with ^");
        }

        return root;
    }

    public override GridCoordinate Walk(GridCoordinate coordinate, InfiniteGrid<bool> grid)
    {
        grid[coordinate] = true;

        foreach (var element in Elements)
        {
            coordinate = element.Walk(coordinate, grid);
        }

        return coordinate;
    }

    public override string ToString()
    {
        return $"^{string.Join('|', Elements)}$";
    }
}
