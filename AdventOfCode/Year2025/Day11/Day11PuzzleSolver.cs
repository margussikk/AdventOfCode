using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2025.Day11;

[Puzzle(2025, 11, "")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyDictionary<string, GraphVertex> _vertices = new Dictionary<string, GraphVertex>();

    public void ParseInput(string[] inputLines)
    {
        var graphBuilder = new GraphBuilder();

        foreach (var line in inputLines)
        {
            var parts = line.Split(':');
            var sourceName = parts[0];
            var destinationNames = parts[1].Split(" ");

            foreach (var destinationName in destinationNames)
            {
                graphBuilder.AddConnection(sourceName, GraphVertexPort.Any, destinationName, GraphVertexPort.Any, 0);
            }
        }

        _vertices = graphBuilder.Vertices;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = Traverse([], _vertices["you"], "out");

        return new PuzzleAnswer(answer, 494);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var dacVertex = _vertices["dac"];
        var fftVertex = _vertices["fft"];

        // Determine dac and fft order
        var checkpoint1Vertex = dacVertex;
        var checkpoint2Vertex = fftVertex;

        var answer = Traverse([], checkpoint1Vertex, checkpoint2Vertex.Name);
        if (answer == 0)
        {
            checkpoint1Vertex = fftVertex;
            checkpoint2Vertex = dacVertex;

            answer = Traverse([], checkpoint1Vertex, checkpoint2Vertex.Name);
            if (answer == 0)
            {
                throw new InvalidOperationException("No connection between dac and fft");
            }
        }

        answer *= Traverse([], _vertices["svr"], checkpoint1Vertex.Name);
        answer *= Traverse([], checkpoint2Vertex, "out");

        return new PuzzleAnswer(answer, 296006754704850L);
    }

    private static long Traverse(Dictionary<string, long> cache, GraphVertex vertex, string outputVertexName)
    {
        if (vertex.Name == outputVertexName)
        {
            return 1;
        }

        if (cache.TryGetValue(vertex.Name, out var paths))
        {
            return paths;
        }

        foreach (var destinationVertex in vertex.Edges.Where(e => e.SourceVertex == vertex).Select(e => e.DestinationVertex))
        {
            paths += Traverse(cache, destinationVertex, outputVertexName);
        }

        cache[vertex.Name] = paths;
        return paths;
    }
}