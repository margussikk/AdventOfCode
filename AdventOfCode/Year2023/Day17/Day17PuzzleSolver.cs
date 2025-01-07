using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2023.Day17;

[Puzzle(2023, 17, "Clumsy Crucible")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private Grid<int> _cityBlockHeatLosses = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _cityBlockHeatLosses = inputLines.SelectToGrid(character => character - '0');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = ModifiedDijkstra(1, 3);

        return new PuzzleAnswer(answer, 665);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = ModifiedDijkstra(4, 10);

        return new PuzzleAnswer(answer, 809);
    }


    private int ModifiedDijkstra(int minSteps, int maxSteps)
    {
        var crucibles = new PriorityQueue<Crucible, int>();

        var startCoordinate = new GridCoordinate(0, 0);
        var endCoordinate = new GridCoordinate(_cityBlockHeatLosses.LastRowIndex, _cityBlockHeatLosses.LastColumnIndex);

        var visited = new HashSet<(GridCoordinate, GridDirection, int)>();

        var crucible = new Crucible(startCoordinate, GridDirection.None, 0, 0);
        crucibles.Enqueue(crucible, 0);

        while (crucibles.TryDequeue(out crucible, out _))
        {
            if (crucible.Coordinate == endCoordinate)
            {
                return crucible.HeatLoss;
            }

            if (!visited.Add(crucible.State))
            {
                continue;
            }

            var previousCoordinate = crucible.Coordinate.Move(crucible.Direction.Flip());
            foreach (var neighborCell in _cityBlockHeatLosses.SideNeighbors(crucible.Coordinate).Where(c => c.Coordinate != previousCoordinate))
            {
                var newDirection = crucible.Coordinate.DirectionToward(neighborCell.Coordinate);

                if ((newDirection == crucible.Direction || crucible.Steps < minSteps) &&
                    ((newDirection != crucible.Direction && crucible.Direction != GridDirection.None) ||
                     crucible.Steps >= maxSteps)) continue;

                var newSteps = newDirection != crucible.Direction ? 1 : crucible.Steps + 1;
                var newHeatLoss = crucible.HeatLoss + neighborCell.Object;

                var newCrucible = new Crucible(neighborCell.Coordinate, newDirection, newSteps, newHeatLoss);

                var distance = neighborCell.Coordinate.ManhattanDistanceTo(endCoordinate);
                crucibles.Enqueue(newCrucible, newHeatLoss + distance);
            }
        }

        return 0;
    }
}
