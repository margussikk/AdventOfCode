using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2018.Day03;

[Puzzle(2018, 3, "No Matter How You Slice It")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private List<Elf> _elves = [];

    public void ParseInput(string[] inputLines)
    {
        _elves = inputLines.Select(Elf.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _elves.GetPairs()
                           .SelectMany(x => x.First.TryFindOverlappingClaimArea(x.Second, out var overlapArea) ? overlapArea.AsEnumerable() : [])
                           .Distinct()
                           .Count();

        return new PuzzleAnswer(answer, 118223);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var overlappedClaimIds = _elves.GetPairs()
                                       .Where(x => x.First.ClaimRegion.Overlaps(x.Second.ClaimRegion))
                                       .SelectMany(x => new int[] { x.First.Id, x.Second.Id })
                                       .Distinct();

        var answer = _elves.Select(e => e.Id)
                           .Except(overlappedClaimIds)
                           .FirstOrDefault();

        return new PuzzleAnswer(answer, 412);
    }
}