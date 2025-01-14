using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.PathFinding;
using AdventOfCode.Year2017.Common;
using System.Numerics;

namespace AdventOfCode.Year2017.Day14;

[Puzzle(2017, 14, "Disk Defragmentation")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0;

        for (var i = 0; i < 128; i++)
        {
            var knotHash = Hash.KnotHash(_input + "-" + i);
            answer += Convert.FromHexString(knotHash).Sum(b => BitOperations.PopCount(b));
        }

        return new PuzzleAnswer(answer, 8250);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var grid = new Grid<bool>(128, 128);

        for (var row = grid.FirstRow; row <= grid.LastRow; row++)
        {
            var knotHash = Hash.KnotHash(_input + "-" + row);
            var bytes = Convert.FromHexString(knotHash);

            var columnOffset = 0;
            foreach (var b in bytes)
            {
                for (var column = 0; column < 8; column++)
                {
                    var bitmask = 1 << (7 - column);
                    if ((b & bitmask) == 0)
                    {
                        continue;
                    }

                    grid[row, columnOffset + column] = true;
                }

                columnOffset += 8;
            }
        }

        var pathFinder = new GridPathFinder<bool>(grid);
        var answer = 0;

        foreach (var cell in grid.Where(c => c.Object))
        {
            answer++;

            var floodFill = pathFinder.FloodFill(cell.Coordinate, cell => cell.Object);
            foreach(var coordinate in floodFill.Keys)
            {
                grid[coordinate] = false;
            }
        }

        return new PuzzleAnswer(answer, 1113);
    }
}