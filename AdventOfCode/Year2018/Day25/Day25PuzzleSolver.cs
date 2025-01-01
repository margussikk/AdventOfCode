using AdventOfCode.Framework.Puzzle;
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
        var answer = 0;

        var coordinates = _coordinates.ToList();

        while (coordinates.Count > 0)
        {
            var constellation = new List<Coordinate4D>();
            bool addedMembers;

            do
            {
                var newCoordinates = new List<Coordinate4D>();
                addedMembers = false;

                foreach (var coordinate in coordinates)
                {
                    if (constellation.Count == 0 || constellation.Any(c => c.ManhattanDistanceTo(coordinate) <= 3))
                    {
                        constellation.Add(coordinate);
                        addedMembers = true;
                    }
                    else
                    {
                        newCoordinates.Add(coordinate);
                    }
                }

                coordinates = newCoordinates;
            } while (addedMembers);

            answer++;
        }

        return new PuzzleAnswer(answer, 396);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}