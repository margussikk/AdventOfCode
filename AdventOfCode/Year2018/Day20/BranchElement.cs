using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2018.Day20;
internal class BranchElement : RouteElement
{
    public static BranchElement Parse(ref ReadOnlySpan<char> span)
    {
        var branch = new BranchElement();

        while (span.Length > 0)
        {
            if (span[0] is '(' or '|')
            {
                span = span[1..];

                var container = ContainerElement.Parse(ref span);

                if (container.Elements.Count == 1)
                {
                    branch.Elements.Add(container.Elements[0]);
                }
                else
                {
                    branch.Elements.Add(container);
                }
            }

            if (span[0] is ')' or '$')
            {
                span = span[1..];
                break;
            }
        }

        return branch;
    }

    public override GridCoordinate Walk(GridCoordinate coordinate, InfiniteGrid<bool> grid)
    {
        foreach (var element in Elements)
        {
            element.Walk(coordinate, grid);
        }

        return coordinate;
    }

    public override string ToString()
    {
        return $"({string.Join('|', Elements)})";
    }
}
