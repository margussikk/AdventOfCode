using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Graph;

namespace AdventOfCode.Year2023.Day25;

[Puzzle(2023, 25, "Snowverload")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<GraphVertex> _components = [];
    //private IReadOnlyList<GraphEdge> _wires = [];

    public void ParseInput(string[] inputLines)
    {
        var graphBuilder = new GraphBuilder();

        foreach (var line in inputLines)
        {
            var splits = line.Split(':');

            var mainComponentName = splits[0].Trim();
            var otherComponentNames = splits[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var otherComponentName in otherComponentNames)
            {
                graphBuilder.AddConnection(mainComponentName, GraphVertexPort.Any, otherComponentName, GraphVertexPort.Any, 1);
            }
        }

        _components = graphBuilder.GetVertices();
        //_wires = graphBuilder.GetEdges();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var numberOfWiresToCut = 3;

        int answer = 0;

        var notEnoughWires = _components.Any(v => v.Edges.Count <= numberOfWiresToCut);
        if (notEnoughWires)
        {
            throw new InvalidOperationException($"Solution requires all components to have more than 3 wires. There needs to be at least 1 extra path exiting the start component but not reaching the end component.");
        }

        // Find all the paths from the first component to all the other components. Keep track of wires used and don't use them on the next run.
        // Eventually two components are in different groups and these groups are connected by 3 wires. Once these 3 wires are used up, we have reached the end.
        // Since all the components have to have at least one more wire than needs to be cut, visited set contains all the components in start components group,
        // end component is in another group. Size of the first group is same as size of the visited set, second group size is total - first group size.
        var startComponent = _components[0];
        foreach (var endComponent in _components.Skip(1))
        {
            var found = false;
            var visitedComponentIds = new HashSet<int>();
            var usedWireIds = new HashSet<int>();
            var pathsFound = 0;

            // Find multiple paths between the same start and end component, but don't use the same wire twice. Eventually there are no available wires left.
            // If components are in different groups, then there are only 3 paths, components in the same group have at least 4 paths between them.
            do
            {
                found = false;
                visitedComponentIds.Clear();

                var componentVisitors = new Queue<ComponentVisitor>();

                var componentVisitor = new ComponentVisitor(startComponent, []);
                componentVisitors.Enqueue(componentVisitor);

                while (componentVisitors.TryDequeue(out componentVisitor))
                {
                    if (componentVisitor.Component == endComponent)
                    {
                        found = true;

                        pathsFound++;

                        // Mark wires as used so not use those on the next runs
                        foreach (var wireId in componentVisitor.WireIds)
                        {
                            usedWireIds.Add(wireId);
                        }
                        break;
                    }

                    if (visitedComponentIds.Contains(componentVisitor.Component.Id))
                    {
                        continue;
                    }

                    visitedComponentIds.Add(componentVisitor.Component.Id);

                    foreach (var wire in componentVisitor.Component.Edges.Where(w => !usedWireIds.Contains(w.Id)))
                    {
                        var nextComponent = wire.SourceVertex == componentVisitor.Component ? wire.DestinationVertex : wire.SourceVertex;

                        var nextQueueItem = new ComponentVisitor(nextComponent, [.. componentVisitor.WireIds, wire.Id]);
                        componentVisitors.Enqueue(nextQueueItem);
                    }
                }
            } while (found);

            if (pathsFound == numberOfWiresToCut)
            {
                //For debugging
                //var groupComponents = visitedComponentIds.ToList();
                //var wiresToCut = _wires // Find wires which connect two groups
                //    .Where(x => (groupComponents.Contains(x.SourceVertex.Id) && !groupComponents.Contains(x.DestinationVertex.Id)) ||
                //                (groupComponents.Contains(x.DestinationVertex.Id) && !groupComponents.Contains(x.SourceVertex.Id)))
                //    .ToList();
                //var wireNames = string.Join(", ", wiresToCut.Select(w => $"{w.SourceVertex.Name}-{w.DestinationVertex.Name}"));
                //Console.WriteLine($"Wires to cut: {wireNames}"); // rcn-xkf, dht-xmv, cms-thk

                answer = visitedComponentIds.Count * (_components.Count - visitedComponentIds.Count);
                break;
            }
        }

        return new PuzzleAnswer(answer, 543036);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}
