using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2018.Day25;

[Puzzle(2018, 25, "Four-Dimensional Adventure")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private List<Coordinate4D> _coordinates = [];

    public void ParseInput(string[] inputLines)
    {
        _coordinates = inputLines.Select(Coordinate4D.Parse)
                                 .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var constellations = new List<List<Coordinate4D>>();
        var coordinateConstellations = new Dictionary<Coordinate4D, List<Coordinate4D>>();

        foreach (var coordinate in _coordinates)
        {
            var list = new List<Coordinate4D> { coordinate };
            constellations.Add(list);
            coordinateConstellations.Add(coordinate, list);
        }

        foreach (var (First, Second) in _coordinates.Pairs().Where(p => p.First.ManhattanDistanceTo(p.Second) <= 3))
        {
            var firstConstellation = coordinateConstellations[First];
            var secondConstellation = coordinateConstellations[Second];

            if (firstConstellation != secondConstellation)
            {
                // Merge second constellation to first
                foreach(var coordinate in secondConstellation)
                {
                    coordinateConstellations[coordinate] = firstConstellation;
                }

                firstConstellation.AddRange(secondConstellation);
                constellations.Remove(secondConstellation);
            }
            else
            {
                // They are already in the same constellation
            }
        }

        return new PuzzleAnswer(constellations.Count, 396);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}