using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2024.Day07;

[Puzzle(2024, 7, "Bridge Repair")]
public class Day07PuzzleSolver : IPuzzleSolver
{
    private List<Equation> _equations = [];

    public void ParseInput(string[] inputLines)
    {
        _equations = inputLines.Select(Equation.Parse)
                               .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _equations.Where(e => e.CouldProduce(false))
                               .Sum(e => e.TestValue);

        return new PuzzleAnswer(answer, 10741443549536L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _equations.Where(e => e.CouldProduce(true))
                               .Sum(e => e.TestValue);

        return new PuzzleAnswer(answer, 500335179214836L);
    }
}