using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day10;

[Puzzle(2024, 10, "Hoof It")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private Grid<int> _map = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _map = inputLines.SelectToGrid(x => x - '0');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _map.Where(c => c.Object == 0)
                         .Sum(c => FindTrailEndCoordinates(c.Coordinate).Distinct().Count());

        return new PuzzleAnswer(answer, 489);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _map.Where(c => c.Object == 0)
                         .Sum(c => FindTrailEndCoordinates(c.Coordinate).Count);

        return new PuzzleAnswer(answer, 1086);
    }

    // BFS
    private List<GridCoordinate> FindTrailEndCoordinates(GridCoordinate startCoordinate)
    {
        var endCoordinates = new List<GridCoordinate>();

        var hiker = new Hiker
        {
            Coordinate = startCoordinate,
            Steps = 0
        };

        var hikerQueue = new Queue<Hiker>();
        hikerQueue.Enqueue(hiker);

        while (hikerQueue.TryDequeue(out hiker))
        {
            if (hiker.Steps == 9)
            {
                endCoordinates.Add(hiker.Coordinate);

                continue;
            }

            foreach (var neighborCell in _map.SideNeighbors(hiker.Coordinate)
                                             .Where(c => c.Object == hiker.Steps + 1))
            {
                var newHiker = new Hiker
                {
                    Coordinate = neighborCell.Coordinate,
                    Steps = hiker.Steps + 1
                };

                hikerQueue.Enqueue(newHiker);
            }
        }

        return endCoordinates;
    }
}