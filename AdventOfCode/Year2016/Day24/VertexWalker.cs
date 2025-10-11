using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2016.Day24;

internal class VertexWalker
{
    public GraphVertex CurrentVertex { get; }

    public int Steps { get; init; }

    public int Numbers { get; set; }

    public VertexWalker(GraphVertex currentVertex)
    {
        CurrentVertex = currentVertex;
    }
}