using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Numerics;
using Microsoft.Diagnostics.Tracing.Parsers.IIS_Trace;

namespace AdventOfCode.Year2020.Day24;

[Puzzle(2020, 24, "Lobby Layout")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var blackTileCoordinates = GetBlackTileCoordinates();

        var answer = blackTileCoordinates.Count;

        return new PuzzleAnswer(answer, 495);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var blackTileCoordinates = GetBlackTileCoordinates();

        var minQ = int.MaxValue;
        var maxQ = int.MinValue;

        var minR = int.MaxValue;
        var maxR = int.MinValue;

        foreach(var coordinate in blackTileCoordinates)
        {
            minQ = int.Min(coordinate.Q, minQ);
            maxQ = int.Max(coordinate.Q, maxQ);

            minR = int.Min(coordinate.R, minR);
            maxR = int.Max(coordinate.R, maxR);
        }

        var qRange = new NumberRange<int>(minQ - 100, maxQ + 100);
        var rRange = new NumberRange<int>(minR - 100, maxR + 100);

        var grid = new HexBitGrid(qRange, rRange);
        foreach(var coordinate in blackTileCoordinates)
        {
            grid[coordinate] = true;
        }

        for (var day = 1; day <= 100; day++)
        {
            var cellUpdates = new List<HexGridCell<bool>>();

            var windowQRange = new NumberRange<int>(minQ - day, maxQ + day);
            var windowRRange = new NumberRange<int>(minR - day, maxR + day);

            foreach (var cell in grid.Window(windowQRange, windowRRange))
            {
                var blackNeighbors = grid.AroundNeighbors(cell.Coordinate)
                                         .Count(c => c.Object);

                if (cell.Object)
                {
                    if (blackNeighbors == 0 || blackNeighbors > 2)
                    {
                        cellUpdates.Add(new HexGridCell<bool>(cell.Coordinate, false));
                    }
                }
                else
                {
                    if (blackNeighbors == 2)
                    {
                        cellUpdates.Add(new HexGridCell<bool>(cell.Coordinate, true));
                    }
                }
            }

            foreach (var cellUpdate in cellUpdates)
            {
                grid[cellUpdate.Coordinate] = cellUpdate.Object;
            }
        }

        var answer = grid.Count(cell => cell.Object);

        return new PuzzleAnswer(answer, 4012);
    }

    private List<HexCoordinate> GetBlackTileCoordinates()
    {
        var blackTileCoordinates = new HashSet<HexCoordinate>();

        foreach (var instruction in _instructions)
        {
            var coordinate = new HexCoordinate(0, 0);

            foreach (var direction in instruction.Directions)
            {
                coordinate = coordinate.Move(direction);
            }

            if (!blackTileCoordinates.Remove(coordinate))
            {
                blackTileCoordinates.Add(coordinate);
            }
        }

        return [.. blackTileCoordinates];
    }
}