namespace AdventOfCode.Utilities.Graph;

internal class GraphBuilder
{
    private int _vertexId = 0;
    private int _edgeId = 0;
    private readonly Dictionary<string, GraphVertex> _vertices = [];
    private readonly List<GraphEdge> _edges = [];

    public void AddConnection(string sourceName, GraphVertexPort sourcePort, string destinationName, GraphVertexPort destinationPort, int weight)
    {
        if (!_vertices.TryGetValue(sourceName, out var sourceVertex))
        {
            sourceVertex = new GraphVertex(++_vertexId, sourceName);
            _vertices[sourceName] = sourceVertex;
        }

        if (!_vertices.TryGetValue(destinationName, out var destinationVertex))
        {
            destinationVertex = new GraphVertex(++_vertexId, destinationName);
            _vertices[destinationName] = destinationVertex;
        }

        var edge = new GraphEdge(++_edgeId, sourceVertex, sourcePort, destinationVertex, destinationPort, weight);
        sourceVertex.Edges.Add(edge);
        destinationVertex.Edges.Add(edge);

        _edges.Add(edge);
    }

    public IReadOnlyList<GraphVertex> GetVertices() => _vertices.Values.ToList();

    public IReadOnlyList<GraphEdge> GetEdges() => _edges;
}
