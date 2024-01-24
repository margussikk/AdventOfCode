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

        var hillClimbers = new PriorityQueue<HillClimber, int>();

        var hillclimber = new HillClimber(startCoordinate, 0);
        hillClimbers.Enqueue(hillclimber, hillclimber.Steps);

        while (hillClimbers.TryDequeue(out hillclimber, out _))
        {
            if (endCondition(hillclimber.Coordinate))
            {
                return hillclimber.Steps;
            }
            else if (shortestDistancesGrid[hillclimber.Coordinate].HasValue)
            {
                continue;
            }

            shortestDistancesGrid[hillclimber.Coordinate] = hillclimber.Steps;

            foreach (var neighborCell in _grid.SideNeighbors(hillclimber.Coordinate))
            {
                var elevationDifference = directionMultiplier * (neighborCell.Object - _grid[hillclimber.Coordinate]);
                if (elevationDifference <= 1)
                {
                    var currentDistance = shortestDistancesGrid[neighborCell.Coordinate] ?? int.MaxValue;
                    if (hillclimber.Steps + 1 < currentDistance)
                    {
                        var newHillClimber = new HillClimber(neighborCell.Coordinate, hillclimber.Steps + 1);
                        hillClimbers.Enqueue(newHillClimber, newHillClimber.Steps);
                    }
                }
            }
        }

        return 0;
    }
}