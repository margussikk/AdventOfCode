using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2021.Day12;

internal class CaveWalker(GraphVertex currentVertex, long visitedBitMask, bool duplicateSmallCaveAllowed)
{
    public GraphVertex CurrentVertex { get; } = currentVertex;

    public long VisitedBitMask { get; set; } = visitedBitMask;

    public bool DuplicateSmallCaveAllowed { get; set; } = duplicateSmallCaveAllowed;

    public bool IsVisitingSmallCave => CurrentVertex.Name[0] >= 'a' && CurrentVertex.Name[0] <= 'z';
}
