using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using BenchmarkDotNet.Columns;
using System.Text;

namespace AdventOfCode.Year2023.Day10;

[Puzzle(2023, 10, "Pipe Maze")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private readonly GridDirection[] _turnPipeDirections =
    [
        GridDirection.Up | GridDirection.Right,
        GridDirection.Up | GridDirection.Left,
        GridDirection.Down | GridDirection.Right,
        GridDirection.Down | GridDirection.Left,
    ];

    private Grid<GridDirection> _pipeDirections = new(0, 0);

    private GridCoordinate _startCoordinate;

    public void ParseInput(List<string> inputLines)
    {
        _pipeDirections = inputLines.SelectToGrid(character => character switch
        {
            '|' => GridDirection.Up | GridDirection.Down,
            '-' => GridDirection.Left | GridDirection.Right,
            'L' => GridDirection.Up | GridDirection.Right,
            'J' => GridDirection.Up | GridDirection.Left,
            '7' => GridDirection.Down | GridDirection.Left,
            'F' => GridDirection.Down | GridDirection.Right,
            '.' => GridDirection.None,
            'S' => GridDirection.Start,
            _ => throw new InvalidOperationException()
        });

        _startCoordinate = _pipeDirections.FindCoordinate(x => x.HasFlag(GridDirection.Start))
            ?? throw new InvalidOperationException("Start coordinate not found");

        _pipeDirections[_startCoordinate] |= DetermineStartPipeDirections();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var steps = 0L;

        TraverseTheLoop(_ => steps++);

        var answer = steps / 2; // Halfway is the furthest distance

        return new PuzzleAnswer(answer, 7086);
    }


    // Solution counting up and down directions
    public PuzzleAnswer GetPartTwoAnswer()
    {
        var loopGrid = new BitGrid(_pipeDirections.Height, _pipeDirections.Width);

        TraverseTheLoop(coordinate => loopGrid[coordinate] = true);

        var answer = 0;

        for (var row = 0; row < loopGrid.Height; row++)
        {
            var upPart = false;
            var downPart = false;

            for (var column = 0; column < loopGrid.Width; column++)
            {
                if (loopGrid[row, column])
                {
                    if (_pipeDirections[row, column].HasFlag(GridDirection.Up))
                    {
                        upPart = !upPart;
                    }

                    if (_pipeDirections[row, column].HasFlag(GridDirection.Down))
                    {
                        downPart = !downPart;
                    }
                }
                else if (upPart && downPart)
                {
                    answer++;
                }
            }
        }

        return new PuzzleAnswer(answer, 317);
    }

    // Solution using shoelace formula and pick's theorem
    public PuzzleAnswer GetPartTwoAnswerV2()
    {
        var turnCoordinates = new List<GridCoordinate>();

        TraverseTheLoop(CollectTurnCoordinates);

        var area = MeasurementFunctions.ShoelaceFormula(turnCoordinates);
        var boundaryPoints = MeasurementFunctions.ManhattanDistanceLoop(turnCoordinates);
        var answer = MeasurementFunctions.PicksTheoremGetInteriorPoints(area, boundaryPoints);

        return new PuzzleAnswer(answer, 317);

        void CollectTurnCoordinates(GridCoordinate coordinate)
        {
            foreach(var direction in _turnPipeDirections)
            {
                if (_pipeDirections[coordinate].HasFlag(direction))
                {
                    turnCoordinates.Add(coordinate);
                    return;
                }
            }
        }
    }

    private void TraverseTheLoop(Action<GridCoordinate> action)
    {
        var direction = new GridDirection[] { GridDirection.Up, GridDirection.Down, GridDirection.Left, GridDirection.Right }
            .First(x => _pipeDirections[_startCoordinate].HasFlag(x));

        var currentCoordinate = _startCoordinate;
        do
        {
            action.Invoke(currentCoordinate);

            // Remove entrance direction, this leaves only the exit direction
            direction = _pipeDirections[currentCoordinate] & ~(GridDirection.Start | direction.Flip());

            currentCoordinate = currentCoordinate.MoveTo(direction);
        }
        while (currentCoordinate != _startCoordinate);
    }

    private GridDirection DetermineStartPipeDirections()
    {
        var startPipeDirections = GridDirection.None;

        foreach(var sideCell in _pipeDirections.Sides(_startCoordinate))
        {
            var relativeDirection = _startCoordinate.RelativeDirection(sideCell.Coordinate);
            if (sideCell.Object.HasFlag(relativeDirection))
            {
                startPipeDirections |= relativeDirection.Flip();
            }
        }

        return startPipeDirections;
    }
}
