using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2020.Day04;

[Puzzle(2020, 4, "Passport Processing")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private List<Passport> _passports = [];

    public void ParseInput(string[] inputLines)
    {
        _passports = inputLines.SelectToChunks()
                               .Select(Passport.Parse)
                               .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _passports.Count(p => p.RequiredFieldsAreFilled());

        return new PuzzleAnswer(answer, 210);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _passports.Count(p => p.IsDataValid());

        return new PuzzleAnswer(answer, 131);
    }
}