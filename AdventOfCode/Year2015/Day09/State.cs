using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2015.Day09;

internal class State
{
    public GraphVertex CurrentVertex { get; set; }

    public int Distance { get; set; }

    public int VisitedBitMask { get; set; }

    public State(GraphVertex currentVertex, int distance)
    {
        CurrentVertex = currentVertex;
        Distance = distance;
    }
}
