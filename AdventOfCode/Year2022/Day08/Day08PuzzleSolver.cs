using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2022.Day08;

[Puzzle(2022, 8, "Treetop Tree House")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private Grid<int> _treeHeights = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _treeHeights = inputLines.SelectToGrid(character => character - '0');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _treeHeights.Count(gridCell =>
            gridCell.Coordinate.Row == 0 ||
            gridCell.Coordinate.Row == _treeHeights.LastRow ||
            gridCell.Coordinate.Column == 0 ||
            gridCell.Coordinate.Column == _treeHeights.LastColumn ||
            IsVisible(gridCell.Coordinate, GridDirection.Up) ||
            IsVisible(gridCell.Coordinate, GridDirection.Down) ||
            IsVisible(gridCell.Coordinate, GridDirection.Left) ||
            IsVisible(gridCell.Coordinate, GridDirection.Right));

        return new PuzzleAnswer(answer, 1809);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _treeHeights.Max(gridCell =>
        {
            var upDistance = GetViewingDistance(gridCell.Coordinate, GridDirection.Up);
            var leftDistance = GetViewingDistance(gridCell.Coordinate, GridDirection.Left);
            var downDistance = GetViewingDistance(gridCell.Coordinate, GridDirection.Down);
            var rightDistance = GetViewingDistance(gridCell.Coordinate, GridDirection.Right);

            return upDistance * leftDistance * downDistance * rightDistance; // Scenic score
        });

        return new PuzzleAnswer(answer, 479400);
    }

    private bool IsVisible(GridCoordinate coordinate, GridDirection direction)
    {
        var testCoordinate = coordinate.Move(direction);
        while (_treeHeights.InBounds(testCoordinate))
        {
            if (_treeHeights[testCoordinate] >= _treeHeights[coordinate])
            {
                return false;
            }

            testCoordinate = testCoordinate.Move(direction);
        }

        return true;
    }

    private int GetViewingDistance(GridCoordinate coordinate, GridDirection direction)
    {
        var distance = 0;

        var testCoordinate = coordinate.Move(direction);
        while (_treeHeights.InBounds(testCoordinate))
        {
            distance++;

            if (_treeHeights[testCoordinate] >= _treeHeights[coordinate])
            {
                return distance;
            }

            testCoordinate = testCoordinate.Move(direction);
        }

        return distance;
    }
}