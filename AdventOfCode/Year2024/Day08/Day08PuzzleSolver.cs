using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day08;

[Puzzle(2024, 8, "Resonant Collinearity")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<char, List<Coordinate2D>> _antennas = [];

    private Region2D _region = new(Coordinate2D.Zero, Coordinate2D.Zero);

    public void ParseInput(string[] inputLines)
    {
        _region = new Region2D(Coordinate2D.Zero, new Coordinate2D(inputLines[0].Length - 1, inputLines.Length - 1)); // - 1 because the end coordinate is inclusive

        foreach (var coordinate in _region)
        {
            var character = inputLines[coordinate.Y][(int)coordinate.X];
            if (character == '.')
            {
                continue;
            }

            if (!_antennas.TryGetValue(character, out var antennaCoordinates))
            {
                antennaCoordinates = [];
                _antennas[character] = antennaCoordinates;
            }

            antennaCoordinates.Add(coordinate);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var antinodeCoordinates = new HashSet<Coordinate2D>();

        foreach (var antenna in _antennas)
        {
            foreach (var (first, second) in antenna.Value.GetPairs())
            {
                CollectAntinode(antinodeCoordinates, first, second, true);
                CollectAntinode(antinodeCoordinates, second, first, true);
            }
        }

        return new PuzzleAnswer(antinodeCoordinates.Count, 308);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var antinodeCoordinates = new HashSet<Coordinate2D>();

        foreach (var antenna in _antennas)
        {
            foreach (var (first, second) in antenna.Value.GetPairs())
            {
                antinodeCoordinates.Add(first);
                antinodeCoordinates.Add(second);

                CollectAntinode(antinodeCoordinates, first, second, false);
                CollectAntinode(antinodeCoordinates, second, first, false);
            }
        }

        return new PuzzleAnswer(antinodeCoordinates.Count, 1147);
    }

    private void CollectAntinode(HashSet<Coordinate2D> antinodeCoordinates, Coordinate2D coordinate1, Coordinate2D coordinate2, bool once)
    {
        var vector = coordinate2 - coordinate1;

        var coordinate = coordinate2 + vector;
        while (_region.InBounds(coordinate))
        {
            antinodeCoordinates.Add(coordinate);

            if (once)
            {
                return;
            }

            coordinate += vector;
        }
    }
}