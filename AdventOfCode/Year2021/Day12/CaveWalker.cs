using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2021.Day12;

internal class CaveWalker
{
    public GraphVertex CurrentVertex { get; }

    public long VisitedBitMask { get; set; }

    public bool DuplicateSmallCaveAllowed { get; set; }

    public bool IsVisitingSmallCave => CurrentVertex.Name[0] >= 'a' && CurrentVertex.Name[0] <= 'z';

    public CaveWalker(GraphVertex currentVertex, long visitedBitMask, bool duplicateSmallCaveAllowed)
    {
        CurrentVertex = currentVertex;
        VisitedBitMask = visitedBitMask;
        DuplicateSmallCaveAllowed = duplicateSmallCaveAllowed;
    }
}
