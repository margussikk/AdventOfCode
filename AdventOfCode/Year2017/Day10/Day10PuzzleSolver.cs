using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2017.Common;

namespace AdventOfCode.Year2017.Day10;

[Puzzle(2017, 10, "Knot Hash")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private string _input = string.Empty;


    public void ParseInput(string[] inputLines)
    {
        _input = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var lengths = _input.SplitToNumbers<byte>(',');

        var sparseHash = Hash.SparseHash(lengths, 1);

        var answer = sparseHash[0] * sparseHash[1];

        return new PuzzleAnswer(answer, 23874);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = Hash.KnotHash(_input);

        return new PuzzleAnswer(answer, "e1a65bfb5a5ce396025fab5528c25a87");
    }
}