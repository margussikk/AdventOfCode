using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using Spectre.Console;

namespace AdventOfCode.Year2025.Day09;

[Puzzle(2025, 9, "Movie Theater")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Coordinate2D> _redTileCoordinates = [];

    public void ParseInput(string[] inputLines)
    {
        _redTileCoordinates = [.. inputLines.Select(Coordinate2D.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _redTileCoordinates
            .Pairs()
            .Select(pair => new Region2D(pair.First, pair.Second))
            .Max(region => region.XLength * region.YLength);

        return new PuzzleAnswer(answer, 4735222687L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0L;

        var sortedRegions = _redTileCoordinates
            .Pairs()
            .Select(pair => new Region2D(pair.First, pair.Second))
            .OrderByDescending(region => region.XLength * region.YLength)
            .ToList();

        var segments = _redTileCoordinates
            .Concat([_redTileCoordinates[0]])
            .SlidingWindow(2)
            .Select(pair => BuildNormalizedLineSegment(pair[0], pair[1]))
            .OrderByDescending(x => x.Start.ManhattanDistanceTo(x.End))
            .ToList();

        foreach (var region in sortedRegions)
        {
            var crossesRegion = false;

            // Horizontal segments
            foreach (var segment in segments.Where(s => s.Start.Y == s.End.Y))
            {
                if (segment.Start.Y <= region.MinCoordinate.Y || segment.Start.Y >= region.MaxCoordinate.Y)
                {
                    // Outside or on the edge
                    continue;
                }

                if (segment.Start.X < region.MaxCoordinate.X && segment.End.X > region.MinCoordinate.X)
                {
                    // Crosses the region
                    crossesRegion = true;
                    break;
                }
            }

            if (crossesRegion)
            {
                continue;
            }

            // Vertical segments
            foreach (var segment in segments.Where(s => s.Start.X == s.End.X))
            {
                if (segment.Start.X <= region.MinCoordinate.X || segment.Start.X >= region.MaxCoordinate.X)
                {
                    // Outside or on the edge
                    continue;
                }

                if (segment.Start.Y < region.MaxCoordinate.Y && segment.End.Y > region.MinCoordinate.Y)
                {
                    // Crosses the region
                    crossesRegion = true;
                    break;
                }
            }

            if (!crossesRegion)
            {
                answer = region.XLength * region.YLength;
                break;
            }
        }

        return new PuzzleAnswer(answer, 1569262188L);
    }

    private static LineSegment2D BuildNormalizedLineSegment(Coordinate2D start, Coordinate2D end)
    {
        return new LineSegment2D(
            new Coordinate2D(
                Math.Min(start.X, end.X),
                Math.Min(start.Y, end.Y)),
            new Coordinate2D(
                Math.Max(start.X, end.X),
                Math.Max(start.Y, end.Y)));
    }
}