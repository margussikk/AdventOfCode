using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2019.Day18;

internal class VertexWalker
{
    public GraphVertex[] CurrentVertices { get; }

    public int Steps { get; init; }

    public int Keys { get; set; }

    public VertexWalker(GraphVertex[] currentVertices)
    {
        CurrentVertices = currentVertices;
    }
}