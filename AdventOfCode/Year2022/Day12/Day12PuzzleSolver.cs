using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2022.Day12;

[Puzzle(2022, 12, "Hill Climbing Algorithm")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private Grid<char> _grid = new(0, 0);

    private GridCoordinate _startCoordinate = new();
    private GridCoordinate _endCoordinate = new();

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(character => character);

        foreach (var gridCell in _grid)
        {
            if (gridCell.Object == 'S')
            {
                _grid[gridCell.Coordinate] = 'a';
                _startCoordinate = gridCell.Coordinate;
            }
            else if (gridCell.Object == 'E')
            {
                _grid[gridCell.Coordinate] = 'z';
                _endCoordinate = gridCell.Coordinate;
            }
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = Dijkstra(1, _startCoordinate, x => x == _endCoordinate);

        return new PuzzleAnswer(answer, 380);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = Dijkstra(-1, _endCoordinate, x => _grid[x] == 'a');

        return new PuzzleAnswer(answer, 375);
    }

    private int Dijkstra(int directionMultiplier, GridCoordinate startCoordinate, Func<GridCoordinate, bool> endCondition)
    {
        var shortestDistancesGrid = new Grid<int?>(_grid.Height, _grid.Width);

        var gridWalkers = new PriorityQueue<GridWalker, int>();

        var gridWalker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);
        gridWalkers.Enqueue(gridWalker, gridWalker.Steps);

        while (gridWalkers.TryDequeue(out gridWalker, out _))
        {
            if (endCondition(gridWalker.CurrentCoordinate))
            {
                return gridWalker.Steps;
            }
            else if (shortestDistancesGrid[gridWalker.CurrentCoordinate].HasValue)
            {
                continue;
            }

            shortestDistancesGrid[gridWalker.CurrentCoordinate] = gridWalker.Steps;

            foreach (var neighborCell in _grid.SideNeighbors(gridWalker.CurrentCoordinate))
            {
                var elevationDifference = directionMultiplier * (neighborCell.Object - _grid[gridWalker.CurrentCoordinate]);
                if (elevationDifference <= 1)
                {
                    var currentDistance = shortestDistancesGrid[neighborCell.Coordinate] ?? int.MaxValue;
                    if (gridWalker.Steps + 1 < currentDistance)
                    {
                        var newGridWalker = gridWalker.Clone();

                        var direction = gridWalker.CurrentCoordinate.DirectionToward(neighborCell.Coordinate);
                        newGridWalker.Move(direction);

                        gridWalkers.Enqueue(newGridWalker, newGridWalker.Steps);
                    }
                }
            }
        }

        return 0;
    }
}