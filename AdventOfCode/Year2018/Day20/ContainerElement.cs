using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2018.Day20;

internal class ContainerElement : RouteElement
{
    public static ContainerElement Parse(ref ReadOnlySpan<char> span)
    {
        var container = new ContainerElement();

        while (span.Length > 0)
        {
            if (span[0] is 'W' or 'N' or 'E' or 'S')
            {
                var sequence = MoveElement.Parse(ref span);
                container.Elements.Add(sequence);
            }

            if (span[0] is '(')
            {
                var branch = BranchElement.Parse(ref span);
                container.Elements.Add(branch);
            }

            if (span[0] is '|' or ')' or '$')
            {
                break;
            }
        }

        return container;
    }

    public override GridCoordinate Walk(GridCoordinate coordinate, InfiniteGrid<bool> grid)
    {
        foreach (var element in Elements)
        {
            coordinate = element.Walk(coordinate, grid);
        }

        return coordinate;
    }

    public override string ToString()
    {
        return string.Join(string.Empty, Elements);
    }
}
