using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Graph;
using AdventOfCode.Utilities.Mathematics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023.Day08;

// Input is designed such that LCM is the correct answer and not generalizable for other inputs.
// Since instructions list loops, nodes lists must also loops for LCM to work.
// Nodes form loops and the cycle length is the same as the steps to get from xxA to the xxZ.
// xxA node is outside of the loop.
// Loops don't intersect and every loop has only one xxZ.
// xxA -> ... -> ... -> XXX -> ... -> ... -> xxZ -> ... -> XXX

[Puzzle(2023, 8, "Haunted Wasteland")]
public partial class Day08PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];
    private IReadOnlyList<GraphVertex> _graphVertices = [];

    public void ParseInput(List<string> inputLines)
    {
        // Instructions
        _instructions = inputLines[0]
            .Select(letter => letter switch
            {
                'L' => Instruction.Left,
                'R' => Instruction.Right,
                _ => throw new InvalidOperationException()
            })
            .ToList();

        // Nodes
        var graphBuilder = new GraphBuilder();

        foreach (var line in inputLines.Skip(2))
        {
            var matches = InputLineRegex().Matches(line);
            if (matches.Count != 1)
            {
                throw new InvalidOperationException("Failed to parse input");
            }
            
            var currentNodeName = matches[0].Groups[1].Value;

            var leftNodeName = matches[0].Groups[2].Value;
            graphBuilder.AddConnection(currentNodeName, GraphVertexPort.Left, leftNodeName, GraphVertexPort.Any, 1);

            var rightNodeName = matches[0].Groups[3].Value;
            graphBuilder.AddConnection(currentNodeName, GraphVertexPort.Right, rightNodeName, GraphVertexPort.Any, 1);            
        }

        _graphVertices = graphBuilder.GetVertices();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var startVertex = _graphVertices.Single(vertex => vertex.Name == "AAA");

        var answer = CountSteps(startVertex, vertex => vertex.Name != "ZZZ");

        return new PuzzleAnswer(answer, 18727);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var stepCounts = _graphVertices
            .Where(vertex => vertex.Name[^1] == 'A')
            .Select(vertex => (long)CountSteps(vertex, v => v.Name[^1] != 'Z'))
            .ToList();

        var answer = MathFunctions.LeastCommonMultiple(stepCounts);

        return new PuzzleAnswer(answer, 18024643846273L);
    }

    private int CountSteps(GraphVertex vertex, Func<GraphVertex, bool> criteria)
    {
        var steps = 0;

        while (criteria(vertex))
        {
            var sourceVertexPort = _instructions[steps % _instructions.Count] == Instruction.Left
                ? GraphVertexPort.Left
                : GraphVertexPort.Right;

            vertex = vertex.Edges.First(e => e.SourceVertex == vertex && e.SourceVertexPort == sourceVertexPort).DestinationVertex;
            steps++;
        }

        return steps;
    }

    [GeneratedRegex("([0-9A-Z]{3}) = \\(([0-9A-Z]{3}), ([0-9A-Z]{3})\\)")]
    private static partial Regex InputLineRegex();
}
