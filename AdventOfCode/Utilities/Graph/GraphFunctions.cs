namespace AdventOfCode.Utilities.Graph;

internal static class GraphFunctions
{
    // https://en.wikipedia.org/wiki/Floyd%E2%80%93Warshall_algorithm
    // In computer science, the Floyd–Warshall algorithm (also known as Floyd's algorithm,
    // the Roy–Warshall algorithm, the Roy–Floyd algorithm, or the WFI algorithm) is an
    // algorithm for finding shortest paths in a directed weighted graph with positive or
    // negative edge weights (but with no negative cycles).
    public static int[,] FloydWarshallAlgorithm(IReadOnlyList<GraphVertex> vertices)
    {
        var distances = new int[vertices.Count, vertices.Count];

        //Fill in the default values
        var infinity = int.MaxValue / 2 - 1;
        for (var i = 0; i < vertices.Count; i++)
        {
            for (var j = 0; j < vertices.Count; j++)
            {
                if (i == j)
                {
                    distances[i, j] = 0;
                }
                else if (vertices[i].Edges.Exists(e => e.SourceVertex.Id == vertices[i].Id && e.DestinationVertex.Id == vertices[j].Id))
                {
                    distances[i, j] = 1;
                }
                else
                {
                    distances[i, j] = infinity;
                }
            }
        }

        for (var k = 0; k < vertices.Count; k++)
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                for (var j = 0; j < vertices.Count; j++)
                {
                    var newDistance = distances[i, k] + distances[k, j];
                    if (newDistance < distances[i, j])
                    {
                        distances[i, j] = newDistance;
                        distances[j, i] = newDistance;
                    }
                }
            }
        }

        return distances;
    }
}
