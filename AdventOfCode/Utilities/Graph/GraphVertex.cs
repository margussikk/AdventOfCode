namespace AdventOfCode.Utilities.Graph;

internal class GraphVertex(int id, string name)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public List<GraphEdge> Edges { get; } = [];
}
