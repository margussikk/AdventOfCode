using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2021.Day05;

[Puzzle(2021, 5, "Hydrothermal Venture")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private readonly List<LineSegment2D> _lineSegments = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var line in inputLines)
        {
            var values = line
                .Split("->")
                .SelectMany(x => x.SplitToNumbers<int>(',', ' '))
                .ToArray();

            var segment = new LineSegment2D(values[0], values[1], values[2], values[3]);
            _lineSegments.Add(segment);
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(false);

        return new PuzzleAnswer(answer, 5576);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(true);

        return new PuzzleAnswer(answer, 18144);
    }

    private int GetAnswer(bool countDiagonal)
    {
        var ventMap = new Dictionary<Coordinate2D, int>();

        foreach (var lineSegment in _lineSegments)
        {
            if (lineSegment.Start.Y == lineSegment.End.Y) // Horizontal
            {
                var startX = Math.Min(lineSegment.Start.X, lineSegment.End.X);
                var endX = Math.Max(lineSegment.Start.X, lineSegment.End.X);

                for (var x = startX; x <= endX; x++)
                {
                    var coordinate = new Coordinate2D(x, lineSegment.Start.Y);

                    var currentCount = ventMap.GetValueOrDefault(coordinate, 0);
                    ventMap[coordinate] = currentCount + 1;
                }
            }
            else if (lineSegment.Start.X == lineSegment.End.X) // Vertical
            {
                var startY = Math.Min(lineSegment.Start.Y, lineSegment.End.Y);
                var endY = Math.Max(lineSegment.Start.Y, lineSegment.End.Y);

                for (var y = startY; y <= endY; y++)
                {
                    var coordinate = new Coordinate2D(lineSegment.Start.X, y);

                    var currentCount = ventMap.GetValueOrDefault(coordinate, 0);
                    ventMap[coordinate] = currentCount + 1;
                }
            }
            else if (countDiagonal) // Diagonal
            {
                var start = lineSegment.Start;
                var end = lineSegment.End;

                if (lineSegment.Start.X > lineSegment.End.X)
                {
                    start = lineSegment.End;
                    end = lineSegment.Start;
                }

                var y = start.Y;
                var yStep = end.Y.CompareTo(start.Y);
                for (var x = start.X; x <= end.X; x++, y += yStep)
                {
                    var coordinate = new Coordinate2D(x, y);

                    var currentCount = ventMap.GetValueOrDefault(coordinate, 0);
                    ventMap[coordinate] = currentCount + 1;
                }
            }
        }

        return ventMap.Values.Count(x => x >= 2);
    }
}