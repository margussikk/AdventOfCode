using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day04;

[Puzzle(2022, 4, "Camp Cleanup")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyCollection<PairOfElves> _pairsOfElves = [];

    public void ParseInput(string[] inputLines)
    {
        _pairsOfElves = inputLines.Select(PairOfElves.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _pairsOfElves
            .Count(x => x.Elf1Sections.IsFullyContained(x.Elf2Sections) ||
                        x.Elf2Sections.IsFullyContained(x.Elf1Sections));

        return new PuzzleAnswer(answer, 651);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _pairsOfElves
                .Count(x => x.Elf1Sections.IsOverlapped(x.Elf2Sections) ||
                            x.Elf2Sections.IsOverlapped(x.Elf1Sections));

        return new PuzzleAnswer(answer, 956);
    }
}
