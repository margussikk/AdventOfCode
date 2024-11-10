using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Graph;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022.Day16;

[Puzzle(2022, 16, "Proboscidea Volcanium")]
public partial class Day16PuzzleSolver : IPuzzleSolver
{
    private Dictionary<string, Valve> _valves = [];

    public void ParseInput(string[] inputLines)
    {
        var valves = new List<Valve>();
        var valveId = 0;

        var graphBuilder = new GraphBuilder();
        foreach (var line in inputLines)
        {
            var matches = InputLineRegex().Matches(line);
            if (matches.Count != 1)
            {
                throw new InvalidOperationException("Failed to parse regex");
            }

            var match = matches[0];

            // Current valve
            var currentValveName = match.Groups[1].Value;
            var currentValveFlowRate = int.Parse(match.Groups[2].Value);

            if (currentValveName == "AA" || currentValveFlowRate > 0)
            {
                var valve = new Valve(++valveId, currentValveName, currentValveFlowRate);
                valves.Add(valve);
            }

            // Leads to
            var leadsToValveNames = match.Groups[3].Value.Split(", ");
            foreach (var leadsToValveName in leadsToValveNames)
            {
                graphBuilder.AddConnection(currentValveName, GraphVertexPort.Any, leadsToValveName, GraphVertexPort.Any, 1);
            }
        }

        Collapse(valves, graphBuilder.Vertices.Values.ToList());
        _valves = valves.ToDictionary(x => x.Name);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var cache = Traverse(30);

        var answer = cache.Max(c => c.Value);

        return new PuzzleAnswer(answer, 2250);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cache = Traverse(26);

        var answer = cache.Aggregate(0,
            (current1, kvp1) => cache
                .Where(kvp2 => (kvp1.Key & kvp2.Key) == 0)
                .Aggregate(current1, (current, kvp2) => int.Max(current, kvp1.Value + kvp2.Value)));

        return new PuzzleAnswer(answer, 3015);
    }

    private Dictionary<int, int> Traverse(int minutesLeft1)
    {
        const int allValvesClosedBitmask = 0;
        var allValvesOpenedBitmask = (1 << _valves.Values.Count(v => v.OpenBitmask != 0)) - 1;

        var openValvesFlowRateCache = new Dictionary<int, int>();

        DFS("AA", minutesLeft1, 0, allValvesClosedBitmask);

        return openValvesFlowRateCache;

        void DFS(string valveName, int minutesLeft, int flowRate, int openValvesBitmask)
        {
            var valve = _valves[valveName];

            var currentMaxValvesFlow = openValvesFlowRateCache.GetValueOrDefault(openValvesBitmask, 0);
            if (flowRate > currentMaxValvesFlow)
            {
                openValvesFlowRateCache[openValvesBitmask] = flowRate;
            }

            if (minutesLeft <= 0 || (openValvesBitmask & allValvesOpenedBitmask) == allValvesOpenedBitmask)
            {
                return;
            }

            foreach (var tunnel in valve.Tunnels)
            {
                var leadsToValve = tunnel.LeadsTo;

                // Not enough time
                var nextMinutesLeft = minutesLeft - tunnel.Distance - 1;
                if (nextMinutesLeft <= 0)
                {
                    continue;
                }

                // Already open
                if ((openValvesBitmask & leadsToValve.OpenBitmask) != 0)
                {
                    continue;
                }

                var nextFlowRate = flowRate + nextMinutesLeft * leadsToValve.FlowRate;
                var nextOpenValvesBitmask = openValvesBitmask | leadsToValve.OpenBitmask;

                DFS(leadsToValve.Name, nextMinutesLeft, nextFlowRate, nextOpenValvesBitmask);
            }
        }
    }

    private static void Collapse(List<Valve> valves, IReadOnlyList<GraphVertex> vertices)
    {
        var distances = GraphFunctions.FloydWarshallAlgorithm(vertices);

        for (var i = 0; i < vertices.Count; i++)
        {
            var sourceValve = valves.Find(v => v.Name == vertices[i].Name);
            if (sourceValve == null) continue;
            
            for (var j = 0; j < vertices.Count; j++)
            {
                if (i == j || vertices[j].Name == "AA")
                {
                    continue;
                }

                var destinationValve = valves.Find(v => v.Name == vertices[j].Name);
                if (destinationValve == null) continue;
                
                var tunnel = new Tunnel(destinationValve, distances[i, j]);
                sourceValve.Tunnels.Add(tunnel);
            }
        }
    }

    // Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
    [GeneratedRegex("Valve ([A-Z]+) has flow rate=(\\d+); tunnels? leads? to valves? (.+)")]
    private static partial Regex InputLineRegex();
}