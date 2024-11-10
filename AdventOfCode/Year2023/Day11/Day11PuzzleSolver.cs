using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day11;

[Puzzle(2023, 11, "Cosmic Expansion")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private List<GridCoordinate> _galaxyCoordinates = [];
    private List<int> _expandingRows = [];
    private List<int> _expandingColumns = [];

    public void ParseInput(string[] inputLines)
    {
        _galaxyCoordinates = inputLines
            .SelectMany((line, row) => line
                .Select((letter, column) => (letter, column))
                .Where(x => x.letter == '#')
                .Select(x => new GridCoordinate(row, x.column))
                .ToList())
            .ToList();

        _expandingRows = Enumerable.Range(0, inputLines.Length)
            .Where(row => _galaxyCoordinates.TrueForAll(g => g.Row != row))
            .ToList();

        _expandingColumns = Enumerable.Range(0, inputLines[0].Length)
            .Where(column => _galaxyCoordinates.TrueForAll(g => g.Column != column))
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetDistance(2L);

        return new PuzzleAnswer(answer, 10077850);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetDistance(1_000_000L);

        return new PuzzleAnswer(answer, 504715068438L);
    }

    private long GetDistance(long expansionFactor)
    {
        var sumOfLengths = 0L;

        foreach (var (galaxy1, galaxy1Index) in _galaxyCoordinates.Select((item, index) => (item, index)).Take(_galaxyCoordinates.Count - 1))
        {
            sumOfLengths += _galaxyCoordinates
                .Skip(galaxy1Index + 1)
                .Sum(galaxy2 =>
                {
                    // Direct
                    var directDistance = MeasurementFunctions.ManhattanDistance(galaxy1, galaxy2);

                    // Expanded
                    var minRow = Math.Min(galaxy1.Row, galaxy2.Row);
                    var maxRow = Math.Max(galaxy1.Row, galaxy2.Row);
                    var expandingRowsBetween = _expandingRows.Count(x => x > minRow && x < maxRow);

                    var minColumn = Math.Min(galaxy1.Column, galaxy2.Column);
                    var maxColumn = Math.Max(galaxy1.Column, galaxy2.Column);
                    var expandingColumnsBetween = _expandingColumns.Count(x => x > minColumn && x < maxColumn);

                    var expandedDistance = (expandingRowsBetween + expandingColumnsBetween) * (expansionFactor - 1);
                    
                    // Total
                    return directDistance + expandedDistance;
                });
        }

        return sumOfLengths;
    }
}
