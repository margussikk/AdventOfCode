namespace AdventOfCode.Utilities.Graph;

// In computer science, the Bron–Kerbosch algorithm is an enumeration algorithm for finding all maximal cliques in an undirected graph.
internal static class BronKerbosch
{
    public static List<List<GraphVertex>> FindCliques(List<GraphVertex> R, List<GraphVertex> P, List<GraphVertex> X)
    {
        var cliques = new List<List<GraphVertex>>();

        if (P.Count == 0 && X.Count == 0)
        {
            cliques.Add(new List<GraphVertex>(R));
        }

        while (P.Count != 0)
        {
            var vertex = P[0];

            List<GraphVertex> newR = [.. R, vertex];
            var newP = P.Intersect(vertex.AdjacentVertices()).ToList();
            var newX = X.Intersect(vertex.AdjacentVertices()).ToList();

            cliques.AddRange(FindCliques(newR, newP, newX));

            P.Remove(vertex);
            X.Add(vertex);
        }
        return cliques;
    }
}
