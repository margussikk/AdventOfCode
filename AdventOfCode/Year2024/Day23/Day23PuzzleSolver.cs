using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2024.Day23;

[Puzzle(2024, 23, "LAN Party")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyDictionary<string, GraphVertex> _vertices = new Dictionary<string, GraphVertex>();

    public void ParseInput(string[] inputLines)
    {
        var graphBuilder = new GraphBuilder();

        foreach (var line in inputLines)
        {
            var splits = line.Split('-');
            graphBuilder.AddConnection(splits[0], GraphVertexPort.Any, splits[1], GraphVertexPort.Any, 0);
        }

        _vertices = graphBuilder.Vertices;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var sets = new HashSet<(string, string, string)>();

        foreach (var vertexKvp in _vertices)
        {
            foreach (var pair in vertexKvp.Value.AdjacentVertices().Pairs())
            {
                if (pair.First.AdjacentVertices().Any(x => x.Name == pair.Second.Name) &&
                    (vertexKvp.Key[0] == 't' || pair.First.Name[0] == 't' || pair.Second.Name[0] == 't'))
                {
                    var set = new string[] { vertexKvp.Key, pair.First.Name, pair.Second.Name }
                        .Order(StringComparer.InvariantCulture)
                        .ToList();

                    sets.Add((set[0], set[1], set[2]));
                }
            }
        }

        return new PuzzleAnswer(sets.Count, 1419);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cliques = BronKerbosch.FindCliques([], _vertices.Values.ToList(), []);

        var clique = cliques.OrderByDescending(x => x.Count).First();

        var answer = string.Join(',', clique.Select(x => x.Name).Order(StringComparer.InvariantCulture));

        return new PuzzleAnswer(answer, "af,aq,ck,ee,fb,it,kg,of,ol,rt,sc,vk,zh");
    }
}