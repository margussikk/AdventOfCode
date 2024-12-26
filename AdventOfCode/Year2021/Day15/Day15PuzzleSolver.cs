using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2021.Day15;

[Puzzle(2021, 15, "Chiton")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private Grid<byte> _riskLevelGrid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _riskLevelGrid = inputLines.SelectToGrid(character => Convert.ToByte(character - '0'));
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = Dijkstra(_riskLevelGrid);

        return new PuzzleAnswer(answer, 717);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var grid = new Grid<byte>(_riskLevelGrid.Height * 5, _riskLevelGrid.Width * 5);

        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                var value = _riskLevelGrid[row % _riskLevelGrid.Height, column % _riskLevelGrid.Width] +
                            row / _riskLevelGrid.Height +
                            column / _riskLevelGrid.Width;
                value = value > 9 ? value % 10 + 1 : value;

                grid[row, column] = Convert.ToByte(value);
            }
        }

        var answer = Dijkstra(grid);

        return new PuzzleAnswer(answer, 2993);
    }

    private static int Dijkstra(Grid<byte> grid)
    {
        var totalRiskGrid = new Grid<int?>(grid.Height, grid.Width);
        var queue = new PriorityQueue<CaveWalker, int>();

        var startCoordinate = new GridCoordinate(0, 0);
        var endCoordinate = new GridCoordinate(grid.LastRowIndex, grid.LastColumnIndex);

        var currentCaveWalker = new CaveWalker(startCoordinate, 0);

        queue.Enqueue(currentCaveWalker, 0);
        while (queue.TryDequeue(out currentCaveWalker, out _))
        {
            if (currentCaveWalker.Coordinate == endCoordinate)
            {
                return currentCaveWalker.Distance;
            }

            if (totalRiskGrid[currentCaveWalker.Coordinate].HasValue)
            {
                continue;
            }

            totalRiskGrid[currentCaveWalker.Coordinate] = currentCaveWalker.Distance;

            foreach (var neighbor in grid.SideNeighbors(currentCaveWalker.Coordinate))
            {
                var currentLowestTotalRisk = totalRiskGrid[neighbor.Coordinate] ?? int.MaxValue;

                var newTotalRisk = currentCaveWalker.Distance + neighbor.Object;
                if (newTotalRisk >= currentLowestTotalRisk) continue;

                var newCaveWalker = new CaveWalker(neighbor.Coordinate, newTotalRisk);

                var manhattan = newCaveWalker.Coordinate.ManhattanDistanceBetween(endCoordinate);
                queue.Enqueue(newCaveWalker, newTotalRisk + manhattan);
            }
        }

        return 0;
    }
}