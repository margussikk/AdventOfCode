using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2023.Day23;

internal class VertexHiker(GraphVertex currentVertex, int distance, long visitedBitMask)
{
    public GraphVertex CurrentVertex { get; } = currentVertex;

    public int Distance { get; set; } = distance;

    public long VisitedBitMask { get; set; } = visitedBitMask;
}
