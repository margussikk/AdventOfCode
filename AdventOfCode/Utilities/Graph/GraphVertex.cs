namespace AdventOfCode.Utilities.Graph;

internal class GraphVertex(int id, string name)
{
    public int Id { get; } = id;

    public string Name { get; } = name;

    public object? Object { get; set; }

    public List<GraphEdge> Edges { get; } = [];

    public List<GraphVertex> AdjacentVertices()
    {
        return Edges.Select(x => x.SourceVertex == this ? x.DestinationVertex : x.SourceVertex)
                    .ToList();
    }
}
