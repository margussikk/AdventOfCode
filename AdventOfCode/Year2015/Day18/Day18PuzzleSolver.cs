using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Simulation;

namespace AdventOfCode.Year2015.Day18;

[Puzzle(2015, 18, "Like a GIF For Your Yard")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private Grid<bool> _inputGrid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _inputGrid = inputLines.SelectToGrid(character => character == '#');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(false);

        return new PuzzleAnswer(answer, 1061);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(true);

        return new PuzzleAnswer(answer, 1006);
    }

    private int GetAnswer(bool cornersStuckOn)
    {
        var fixedCoordinates = Array.Empty<GridCoordinate>();

        var gameOfLife = new GameOfLife<Light>(_inputGrid.Height, _inputGrid.Width);
        foreach (var cell in _inputGrid)
        {
            gameOfLife.SetState(cell.Coordinate, cell.Object ? Light.On : Light.Off);
        }

        if (cornersStuckOn)
        {
            fixedCoordinates =
            [
                new GridCoordinate(0, 0),
                new GridCoordinate(0, gameOfLife.LastColumnIndex),
                new GridCoordinate(gameOfLife.LastRowIndex, 0),
                new GridCoordinate(gameOfLife.LastRowIndex, gameOfLife.LastColumnIndex)
            ];

            foreach (var cornerCoordinate in fixedCoordinates)
            {
                gameOfLife.SetState(cornerCoordinate, Light.On);
            }
        }

        var nextGameOfLife = gameOfLife;

        for (var step = 0; step < 100; step++)
        {
            nextGameOfLife = Animate(nextGameOfLife, fixedCoordinates);
        }

        return nextGameOfLife.Count(Light.On);
    }

    private static GameOfLife<Light> Animate(GameOfLife<Light> gameOfLife, GridCoordinate[] fixedCoordinates)
    {
        var nextGameOfLife = gameOfLife.Clone();

        foreach (var cell in gameOfLife.Where(cell => !fixedCoordinates.Contains(cell.Coordinate)))
        {
            if (cell.Object == Light.On)
            {
                if (cell.AroundCount(Light.On) is not (2 or 3))
                {
                    nextGameOfLife.SetState(cell.Coordinate, Light.Off);
                }
            }
            else if (cell.Object == Light.Off)
            {
                if (cell.AroundCount(Light.On) == 3)
                {
                    nextGameOfLife.SetState(cell.Coordinate, Light.On);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        return nextGameOfLife;
    }
}