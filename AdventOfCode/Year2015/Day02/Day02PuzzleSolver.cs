using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day02;

[Puzzle(2015, 2, "I Was Told There Would Be No Math")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Present> _presents = [];

    public void ParseInput(string[] inputLines)
    {
        _presents = [.. inputLines.Select(Present.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _presents.Sum(p => p.GetSquareFeetOfWrappingPaper());

        return new PuzzleAnswer(answer, 1588178);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _presents.Sum(p => p.GetFeetOfRibbon());

        return new PuzzleAnswer(answer, 3783758);
    }
}