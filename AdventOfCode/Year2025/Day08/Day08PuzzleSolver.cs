using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2025.Day08;

[Puzzle(2025, 8, "Playground")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Coordinate3D> _coordinates = [];

    public void ParseInput(string[] inputLines)
    {
        _coordinates = [.. inputLines.Select(Coordinate3D.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var (circuits, _) = ConnectJunctionBoxes(1000);

        var answer = circuits.OrderByDescending(x => x.Count)
                             .Take(3)
                             .Aggregate(1, (agg, curr) => agg * curr.Count);

        return new PuzzleAnswer(answer, 164475);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var (_, lastConnection) = ConnectJunctionBoxes(int.MaxValue);

        var answer = lastConnection.Position1.X * lastConnection.Position2.X;

        return new PuzzleAnswer(answer, 169521198);
    }

    private (List<HashSet<Coordinate3D>> Circuits, Connection LastConnection) ConnectJunctionBoxes(int connectionCount)
    {
        var connection = new Connection(Coordinate3D.Zero, Coordinate3D.Zero);

        var circuits = _coordinates.Select(c => new HashSet<Coordinate3D> { c }).ToList();
        var connections = _coordinates
            .Pairs()
            .Select(pair => new Connection(pair.First, pair.Second))
            .OrderBy(c => c.Distance)
            .ToList();

        for (var index = 0; index < connectionCount && circuits.Count != 1; index++)
        {
            connection = connections[index];

            var circuit1 = circuits.First(circuit => circuit.Contains(connection.Position1));
            var circuit2 = circuits.First(circuit => circuit.Contains(connection.Position2));

            if (circuit1 != circuit2)
            {
                circuit1.UnionWith(circuit2);
                circuits.Remove(circuit2);
            }
        }

        return (circuits, connection);
    }

    private sealed class Connection
    {
        public Coordinate3D Position1 { get; }
        public Coordinate3D Position2 { get; }
        public double Distance { get; }

        public Connection(Coordinate3D position1, Coordinate3D position2)
        {
            Position1 = position1;
            Position2 = position2;
            Distance = position1.EuclideanDistanceTo(position2);
        }
    }
}