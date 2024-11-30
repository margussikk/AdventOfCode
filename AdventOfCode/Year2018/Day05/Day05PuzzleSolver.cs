using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2018.Day05;

[Puzzle(2018, 5, "Alchemical Reduction")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private string _polymer = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _polymer = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetReactedPolymerLength(Convert.ToChar(0));

        return new PuzzleAnswer(answer, 10496);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = Enumerable.Range('a', 'z' - 'a' + 1)
            .Min(unit => GetReactedPolymerLength(Convert.ToChar(unit)));

        return new PuzzleAnswer(answer, 5774);
    }

    private int GetReactedPolymerLength(char removedUnit)
    {
        var unitStack = new Stack<char>();

        foreach (var unit in _polymer.Where(u => char.ToLower(u) != removedUnit))
        {
            if (unitStack.TryPeek(out var lastUnit) &&
                unit != lastUnit && char.ToLower(unit) == char.ToLower(lastUnit))
            {
                unitStack.Pop();
                continue;
            }

            unitStack.Push(unit);
        }

        return unitStack.Count;
    }
}