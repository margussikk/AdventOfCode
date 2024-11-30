using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2021.Day12;

[Puzzle(2021, 12, "Passage Pathing")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private GraphVertex _startVertex = new(0, string.Empty);

    public void ParseInput(string[] inputLines)
    {
        var graphBuilder = new GraphBuilder();

        foreach (var line in inputLines)
        {
            var splits = line.Split('-');

            graphBuilder.AddConnection(splits[0], GraphVertexPort.Any, splits[1], GraphVertexPort.Any, 0);
        }

        _startVertex = graphBuilder.Vertices["start"];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(false);

        return new PuzzleAnswer(answer, 3563);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(true);

        return new PuzzleAnswer(answer, 105453);
    }


    private int GetAnswer(bool duplicateSmallCaveAllowed)
    {
        var answer = 0;

        var queue = new Queue<CaveWalker>();

        var caveWalker = new CaveWalker(_startVertex, 0, duplicateSmallCaveAllowed);
        queue.Enqueue(caveWalker);

        while (queue.TryDequeue(out caveWalker))
        {
            if (caveWalker.IsVisitingSmallCave)
            {
                if (caveWalker.CurrentVertex.Name == "end")
                {
                    answer++;
                    continue;
                }

                var vertexBitMask = 1L << caveWalker.CurrentVertex.Id;
                if ((caveWalker.VisitedBitMask & vertexBitMask) == vertexBitMask)
                {
                    if (caveWalker.DuplicateSmallCaveAllowed)
                    {
                        caveWalker.DuplicateSmallCaveAllowed = false;
                    }
                    else
                    {
                        // Visit the small cave only once
                        continue;
                    }
                }

                caveWalker.VisitedBitMask |= vertexBitMask;
            }

            foreach (var edge in caveWalker.CurrentVertex.Edges)
            {
                var destinationVertex = edge.SourceVertex == caveWalker.CurrentVertex ? edge.DestinationVertex : edge.SourceVertex;
                if (destinationVertex.Name == "start") continue;

                var newCaveWalker = new CaveWalker(destinationVertex, caveWalker.VisitedBitMask, caveWalker.DuplicateSmallCaveAllowed);
                queue.Enqueue(newCaveWalker);
            }
        }

        return answer;
    }
}