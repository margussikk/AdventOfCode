namespace AdventOfCode.Utilities.Graph;

internal class GraphEdge(int id, GraphVertex sourceVertex, GraphVertexPort sourceVertexPort, GraphVertex destinationVertex, GraphVertexPort destinationVertexPort, int weight)
{
    public int Id { get; } = id;

    public GraphVertex SourceVertex { get; } = sourceVertex;

    public GraphVertexPort SourceVertexPort { get; } = sourceVertexPort;

    public GraphVertex DestinationVertex { get; } = destinationVertex;

    public GraphVertexPort DestinationVertexPort { get; } = destinationVertexPort;

    public int Weight { get; } = weight;
}
