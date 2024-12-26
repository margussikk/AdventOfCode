using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day20;

[Puzzle(2024, 20, "Race Condition")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private BitGrid _racetrack = new(0, 0);
    private GridCoordinate _startCoordinate;
    private GridCoordinate _endCoordinate;

    public void ParseInput(string[] inputLines)
    {
        _racetrack = inputLines.SelectToBitGrid((character, coordinate) =>
        {
            if (character == 'S')
            {
                _startCoordinate = coordinate;
            }
            else if (character == 'E')
            {
                _endCoordinate = coordinate;
            }

            return character == '#';
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(2);

        return new PuzzleAnswer(answer, 1384);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(20);

        return new PuzzleAnswer(answer, 1008542);
    }

    private int GetAnswer(int cheatDuration)
    {
        var answer = 0;

        var distancesToEndGrid = BuildDistancesToEndGrid();

        foreach (var startCell in distancesToEndGrid.Where(cell => cell.Object != int.MaxValue))
        {
            for (var distance = 2; distance <= cheatDuration; distance++)
            {
                var endCells = startCell.Coordinate
                    .ManhattanCoordinates(distance)
                    .Where(distancesToEndGrid.InBounds)
                    .Select(distancesToEndGrid.Cell)
                    .Where(cell => cell.Object < startCell.Object)
                    .ToList();

                foreach (var endCell in endCells)
                {
                    var cheatDistance = startCell.Coordinate.ManhattanDistanceBetween(endCell.Coordinate);

                    var savedTime = startCell.Object - endCell.Object - cheatDistance;
                    if (savedTime >= 100)
                    {
                        answer++;
                    }
                }
            }
        }

        return answer;
    }

    private Grid<int> BuildDistancesToEndGrid()
    {
        var distancesToEndGrid = new Grid<int>(_racetrack.Height, _racetrack.Width);

        foreach (var cell in _racetrack)
        {
            distancesToEndGrid[cell.Coordinate] = cell.Object ? int.MaxValue : 0;
        }

        var previousCoordinate = _endCoordinate;
        var coordinate = _endCoordinate;

        var distance = 0;
        while (true)
        {
            distancesToEndGrid[coordinate] = distance;

            var nextCoordinate = _racetrack.SideNeighbors(coordinate)
                                           .Where(c => !c.Object && c.Coordinate != previousCoordinate)
                                           .Select(c => c.Coordinate)
                                           .Cast<GridCoordinate?>()
                                           .FirstOrDefault();
            if (nextCoordinate != null)
            {
                previousCoordinate = coordinate;
                coordinate = nextCoordinate.Value;
            }
            else if (coordinate == _startCoordinate)
            {
                break;
            }
            else
            {
                throw new InvalidOperationException("Couldn't find next coordinate, but it wasn't start either");
            }

            distance++;
        }

        return distancesToEndGrid;
    }
}