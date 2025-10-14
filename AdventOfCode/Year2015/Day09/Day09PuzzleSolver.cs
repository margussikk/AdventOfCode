using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2015.Day09;

[Puzzle(2015, 9, "All in a Single Night")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private readonly GraphBuilder _graphBuilder = new();

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var parts = line.Split(' ');
            _graphBuilder.AddConnection(parts[0], GraphVertexPort.Any, parts[2], GraphVertexPort.Any, int.Parse(parts[4]));
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var visitedAllBitMask = 0;

        var queue = new PriorityQueue<State, int>();
        foreach (var vertex in _graphBuilder.Vertices.Values)
        {
            var state = new State(vertex, 0)
            {
                VisitedBitMask = 1 << vertex.Id
            };
            visitedAllBitMask |= state.VisitedBitMask;

            queue.Enqueue(state, state.Distance);
        }

        var answer = 0;

        while (queue.TryDequeue(out var state, out _))
        {
            if (state.VisitedBitMask == visitedAllBitMask)
            {
                answer = state.Distance;
                break;
            }

            foreach (var edge in state.CurrentVertex.Edges)
            {
                var destinationVertex = edge.SourceVertex == state.CurrentVertex ? edge.DestinationVertex : edge.SourceVertex;

                var bitmask = 1 << destinationVertex.Id;
                if ((state.VisitedBitMask & bitmask) != 0) continue;

                var nextState = new State(destinationVertex, state.Distance + edge.Weight)
                {
                    VisitedBitMask = state.VisitedBitMask | bitmask
                };

                queue.Enqueue(nextState, nextState.Distance);
            }
        }

        return new PuzzleAnswer(answer, 251);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var visitedAllBitMask = 0;

        var queue = new Queue<State>();
        foreach (var vertex in _graphBuilder.Vertices.Values)
        {
            var state = new State(vertex, 0)
            {
                VisitedBitMask = 1 << vertex.Id
            };
            visitedAllBitMask |= state.VisitedBitMask;

            queue.Enqueue(state);
        }

        var answer = 0;

        while (queue.TryDequeue(out var state))
        {
            if (state.VisitedBitMask == visitedAllBitMask)
            {
                answer = int.Max(answer, state.Distance);
                continue;
            }

            foreach (var edge in state.CurrentVertex.Edges)
            {
                var destinationVertex = edge.SourceVertex == state.CurrentVertex ? edge.DestinationVertex : edge.SourceVertex;

                var bitmask = 1 << destinationVertex.Id;
                if ((state.VisitedBitMask & bitmask) != 0) continue;

                var nextState = new State(destinationVertex, state.Distance + edge.Weight)
                {
                    VisitedBitMask = state.VisitedBitMask | bitmask
                };

                queue.Enqueue(nextState);
            }
        }

        return new PuzzleAnswer(answer, 898);
    }
}