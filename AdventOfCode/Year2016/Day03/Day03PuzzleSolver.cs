using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2016.Day03;

[Puzzle(2016, 3, "Squares With Three Sides")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<int[]> _triangleSides = [];

    public void ParseInput(string[] inputLines)
    {
        _triangleSides = [.. inputLines.Select(line => line.SplitToNumbers<int>(' '))];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _triangleSides.Count(sides => IsValidTriangle(sides[0], sides[1], sides[2]));

        return new PuzzleAnswer(answer, 917);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _triangleSides
            .Chunk(3)
            .Sum(chunk => (IsValidTriangle(chunk[0][0], chunk[1][0], chunk[2][0]) ? 1 : 0) +
                          (IsValidTriangle(chunk[0][1], chunk[1][1], chunk[2][1]) ? 1 : 0) +
                          (IsValidTriangle(chunk[0][2], chunk[1][2], chunk[2][2]) ? 1 : 0));

        return new PuzzleAnswer(answer, 1649);
    }

    private static bool IsValidTriangle(int a, int b, int c) => a + b > c && b + c > a && c + a > b;
}