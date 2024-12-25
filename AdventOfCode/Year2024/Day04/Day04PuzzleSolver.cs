using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2024.Day04;

[Puzzle(2024, 4, "Ceres Search")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private Grid<char> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(letter => letter);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var directions = GridDirection.None.AroundDirections();

        var letters = new char[] { 'X', 'M', 'A', 'S' };

        var answer = _grid.Where(cell => cell.Object == 'X')
                          .Sum(cell => directions.Count(direction => letters
                              .Select((letter, index) => (Letter: letter, Coordinate: cell.Coordinate.Move(direction, index)))
                              .All(x => _grid.InBounds(x.Coordinate) && _grid[x.Coordinate] == x.Letter)));

        return new PuzzleAnswer(answer, 2370);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        foreach (var cell in _grid.Where(c => c.Object == 'A'))
        {
            var upLeftCoordinate = cell.Coordinate.Move(GridDirection.UpLeft);
            var upRightCoordinate = cell.Coordinate.Move(GridDirection.UpRight);
            var downLeftCoordinate = cell.Coordinate.Move(GridDirection.DownLeft);
            var downRightCoordinate = cell.Coordinate.Move(GridDirection.DownRight);

            if (_grid.InBounds(upLeftCoordinate) && _grid.InBounds(upRightCoordinate) &&
                _grid.InBounds(downLeftCoordinate) && _grid.InBounds(downRightCoordinate) &&

                ((_grid[upLeftCoordinate] == 'M' && _grid[downRightCoordinate] == 'S') ||
                 (_grid[upLeftCoordinate] == 'S' && _grid[downRightCoordinate] == 'M')) &&

                ((_grid[downLeftCoordinate] == 'M' && _grid[upRightCoordinate] == 'S') ||
                 (_grid[downLeftCoordinate] == 'S' && _grid[upRightCoordinate] == 'M')))
            {
                answer++;
            }
        }

        return new PuzzleAnswer(answer, 1908);
    }
}