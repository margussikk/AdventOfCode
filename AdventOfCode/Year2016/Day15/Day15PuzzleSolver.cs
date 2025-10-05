using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2016.Day15;

[Puzzle(2016, 15, "Timing is Everything")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Disc> _discs = [];

    public void ParseInput(string[] inputLines)
    {
        _discs = [.. inputLines.Select(Disc.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(_discs);

        return new PuzzleAnswer(answer, 376777L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var discs = _discs.Append(new Disc
        {
            Id = _discs.Count + 1,
            Positions = 11,
            StartPosition = 0
        })
                          .ToList();

        var answer = GetAnswer(discs);

        return new PuzzleAnswer(answer, 3903937L);
    }

    private static long GetAnswer(IReadOnlyList<Disc> discs)
    {
        var congruences = discs
            .Select(disc => new Congruence(disc.Positions, MathFunctions.Modulo(disc.Positions - disc.StartPosition - disc.Id, disc.Positions)))
            .ToList();

        return ChineseRemainderTheoremSolver.Solve(congruences);
    }
}