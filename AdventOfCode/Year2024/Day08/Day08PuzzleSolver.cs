using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day08;

[Puzzle(2024, 8, "Resonant Collinearity")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private readonly Dictionary<char, List<Coordinate2D>> _antennas = [];

    private int _areaWidth = 0;
    private int _areaHeight = 0;

    public void ParseInput(string[] inputLines)
    {
        _areaHeight = inputLines.Length;
        _areaWidth = inputLines[0].Length;

        for (var row = 0; row < _areaHeight; row++)
        {
            for (var column = 0; column < _areaWidth; column++)
            {
                var symbol = inputLines[row][column];
                if (symbol == '.')
                {
                    continue;
                }

                if (!_antennas.TryGetValue(symbol, out var antennaCoordinates))
                {
                    antennaCoordinates = [];
                    _antennas[symbol] = antennaCoordinates;
                }

                antennaCoordinates.Add(new Coordinate2D(column, row));
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var antinodeCoordinates = new HashSet<Coordinate2D>();

        foreach (var antenna in _antennas)
        {
            foreach(var (first, second) in antenna.Value.GetPairs())
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

    private bool InBounds(Coordinate2D coordinate)
    {
        return coordinate.X >= 0 && coordinate.X < _areaWidth &&
               coordinate.Y >= 0 && coordinate.Y < _areaHeight;
    }

    private void CollectAntinode(HashSet<Coordinate2D> antinodeCoordinates, Coordinate2D coordinate1, Coordinate2D coordinate2, bool once)
    {
        var vector = coordinate2 - coordinate1;

        var coordinate = coordinate2 + vector;
        while (InBounds(coordinate))
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