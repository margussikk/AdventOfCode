using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day12;

[Puzzle(2015, 12, "JSAbacusFramework.io")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private ObjectElement _rootElement = null!;

    public void ParseInput(string[] inputLines)
    {
        var span = inputLines[0].AsSpan();
        _rootElement = ObjectElement.Parse(ref span);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _rootElement.SumOfNumbers(false);

        return new PuzzleAnswer(answer, 111754);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _rootElement.SumOfNumbers(true);

        return new PuzzleAnswer(answer, 65402);
    }
}