using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.PathFinding;
using System.Numerics;

namespace AdventOfCode.Year2024.Day12;

[Puzzle(2024, 12, "Garden Groups")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private Grid<char> _map = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _map = inputLines.SelectToGrid(c => c);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var visitedMap = new Grid<bool>(_map.Height, _map.Width);

        var answer = _map.Where(cell => !visitedMap[cell.Coordinate])
                         .Select(cell => GetGardenRegion(visitedMap, cell.Coordinate))
                         .Sum(CalculatePriceUsingPerimeter);

        return new PuzzleAnswer(answer, 1494342);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var visitedMap = new Grid<bool>(_map.Height, _map.Width);

        var answer = _map.Where(cell => !visitedMap[cell.Coordinate])
                         .Select(cell => GetGardenRegion(visitedMap, cell.Coordinate))
                         .Sum(CalculatePriceUsingSides);

        return new PuzzleAnswer(answer, 893676);
    }

    private Dictionary<GridCoordinate, GridDirection> GetGardenRegion(Grid<bool> visitedMap, GridCoordinate coordinate)
    {
        var gridPathFinder = new GridPathFinder<char>(_map);
        var region = gridPathFinder.FloodFill(coordinate, cell => cell.Object == _map[coordinate]);

        foreach (var key in region.Keys)
        {
            visitedMap[key] = true;
        }

        return region;
    }

    private static int CalculatePriceUsingPerimeter(Dictionary<GridCoordinate, GridDirection> gardenRegion)
    {
        var area = gardenRegion.Count;
        var perimeter = gardenRegion.Where(x => x.Value != GridDirection.None) // Include only plots with fences
                                    .Sum(fences => BitOperations.PopCount((ulong)fences.Value));

        return area * perimeter;
    }

    private static int CalculatePriceUsingSides(Dictionary<GridCoordinate, GridDirection> gardenRegion)
    {
        var allFenceDirections = GridDirection.None.SideDirections();

        var sideCount = 0;

        var fenceMap = gardenRegion.Where(x => x.Value != GridDirection.None)
                                   .ToDictionary(x => x.Key, x => x.Value);

        while (fenceMap.Count > 0)
        {
            var startPlot = fenceMap.OrderBy(x => x.Key.Row)
                                    .ThenBy(x => x.Key.Column)
                                    .First();

            var direction = allFenceDirections.First(x => startPlot.Value.HasFlag(x))
                                              .TurnRight();

            var walker = new FenceWalker(startPlot.Key, direction);

            while (true)
            {
                if (fenceMap.GetValueOrDefault(walker.Coordinate, GridDirection.None).HasFlag(walker.FenceDirection))
                {
                    var blockage = fenceMap[walker.Coordinate].HasFlag(walker.Direction);

                    if (!walker.IsWalking)
                    {
                        walker.IsWalking = true; // Start walking, keep the first fence
                    }
                    else
                    {
                        ClearFence(walker);

                        if (walker.IsAtStart)
                        {
                            break;
                        }
                    }

                    if (blockage)
                    {
                        walker.TurnRight();
                    }
                    else
                    {
                        walker.Step();
                    }
                }
                else
                {
                    // Open area, outer corner
                    walker.TurnLeft();
                    walker.Step();
                }
            }

            sideCount += walker.Turns;
        }

        var area = gardenRegion.Count;

        return area * sideCount;

        void ClearFence(FenceWalker walker)
        {
            var fences = fenceMap[walker.Coordinate].Clear(walker.FenceDirection);
            if (fences == GridDirection.None)
            {
                fenceMap.Remove(walker.Coordinate);
            }
            else
            {
                fenceMap[walker.Coordinate] = fences;
            }
        }
    }
}