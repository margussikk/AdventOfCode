using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Simulation;

namespace AdventOfCode.Year2018.Day18;

[Puzzle(2018, 18, "Settlers of The North Pole")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private Grid<Tile> _grid = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        _grid = inputLines.SelectToGrid(character => character switch
        {
            '.' => Tile.Open,
            '|' => Tile.Trees,
            '#' => Tile.Lumberyard,
            _ => throw new InvalidOperationException()
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var gameOfLife = new GameOfLife<Tile>(_grid.Height, _grid.Width);
        foreach (var cell in _grid)
        {
            gameOfLife.SetState(cell.Coordinate, cell.Object);
        }

        for (var minute = 0; minute < 10; minute++)
        {
            gameOfLife = ApplyStrangeMagic(gameOfLife);
        }

        var answer = CalculateResourceValue(gameOfLife);

        return new PuzzleAnswer(answer, 535522);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        const int totalCycles = 1_000_000_000;
        var answer = 0;
        var strangeMagicCycles = new Dictionary<int, int>();
        var totalResourceValues = new List<int>();

        var gameOfLife = new GameOfLife<Tile>(_grid.Height, _grid.Width);
        foreach (var cell in _grid)
        {
            gameOfLife.SetState(cell.Coordinate, cell.Object);
        }

        for (var cycle = 0; cycle < int.MaxValue; cycle++)
        {
            var hashCode = gameOfLife.GetHashCode();
            if (strangeMagicCycles.TryGetValue(hashCode, out var cycleStart))
            {
                var cycleLength = cycle - cycleStart;
                var reminder = (totalCycles - cycle) % cycleLength;

                answer = totalResourceValues[cycleStart + reminder];
                break;
            }

            strangeMagicCycles[hashCode] = cycle;

            totalResourceValues.Add(CalculateResourceValue(gameOfLife));

            gameOfLife = ApplyStrangeMagic(gameOfLife);
        }

        return new PuzzleAnswer(answer, 210160);
    }

    private static GameOfLife<Tile> ApplyStrangeMagic(GameOfLife<Tile> gameOfLife)
    {
        var nextGameOfLife = gameOfLife.Clone();

        foreach (var cell in gameOfLife)
        {
            if (cell.Object == Tile.Open)
            {
                if (cell.AroundCount(Tile.Trees) >= 3)
                {
                    nextGameOfLife.SetState(cell.Coordinate, Tile.Trees);
                }
            }
            else if (cell.Object == Tile.Trees)
            {
                if (cell.AroundCount(Tile.Lumberyard) >= 3)
                {
                    nextGameOfLife.SetState(cell.Coordinate, Tile.Lumberyard);
                }
            }
            else if (cell.Object == Tile.Lumberyard)
            {
                if (cell.AroundCount(Tile.Lumberyard) == 0 || cell.AroundCount(Tile.Trees) == 0)
                {
                    nextGameOfLife.SetState(cell.Coordinate, Tile.Open);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        return nextGameOfLife;
    }

    private static int CalculateResourceValue(GameOfLife<Tile> gameOfLife)
    {
        var treesCount = 0;
        var lumberyardCount = 0;

        foreach (var cell in gameOfLife)
        {
            if (cell.Object == Tile.Trees)
            {
                treesCount++;
            }
            else if (cell.Object == Tile.Lumberyard)
            {
                lumberyardCount++;
            }
        }

        return treesCount * lumberyardCount;
    }
}