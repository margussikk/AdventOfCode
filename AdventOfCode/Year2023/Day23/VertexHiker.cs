using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2023.Day23;

internal class VertexHiker
{
    public GraphVertex CurrentVertex { get; }

    public int Distance { get; }

    public long VisitedBitMask { get; set; }

    public VertexHiker(GraphVertex currentVertex, int distance, long visitedBitMask)
    {
        CurrentVertex = currentVertex;
        Distance = distance;
        VisitedBitMask = visitedBitMask;
    }
}
