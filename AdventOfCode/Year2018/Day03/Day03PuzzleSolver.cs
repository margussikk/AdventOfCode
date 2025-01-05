using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

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
        GetAnswerUsingGrid(out var answer, out _);

        return new PuzzleAnswer(answer, 118223);
    }

    public PuzzleAnswer GetPartOneAnswerUsingRegions()
    {
        var answer = _elves.Pairs()
                           .SelectMany(x => x.First.TryFindOverlappingClaimArea(x.Second, out var overlapArea) ? overlapArea.AsEnumerable() : [])
                           .Distinct()
                           .Count();

        return new PuzzleAnswer(answer, 118223);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        GetAnswerUsingGrid(out _, out var answer);

        return new PuzzleAnswer(answer, 412);
    }

    public PuzzleAnswer GetPartTwoAnswerUsingRegionOverlaps()
    {
        var overlappedClaimIds = _elves.Pairs()
                                       .Where(x => x.First.ClaimArea.Overlaps(x.Second.ClaimArea))
                                       .SelectMany(x => new int[] { x.First.Id, x.Second.Id })
                                       .Distinct();

        var answer = _elves.Select(e => e.Id)
                           .Except(overlappedClaimIds)
                           .FirstOrDefault();

        return new PuzzleAnswer(answer, 412);
    }

    public void GetAnswerUsingGrid(out int _overlappedClaims, out int claimWithoutOverlap)
    {
        var grid = new Grid<int?>(1000, 1000);
        _overlappedClaims = 0;

        var elfIds = new HashSet<int>();

        foreach (var elf in _elves)
        {
            elfIds.Add(elf.Id);

            foreach (var coordinate2D in elf.ClaimArea)
            {
                var gridCoordinate = new GridCoordinate((int)coordinate2D.Y, (int)coordinate2D.X);

                if (!grid[gridCoordinate].HasValue)
                {
                    // First claim
                    grid[gridCoordinate] = elf.Id;
                }
                else if (grid[gridCoordinate]!.Value > 0)
                {
                    // First claim overlap
                    elfIds.Remove(grid[gridCoordinate]!.Value);
                    elfIds.Remove(elf.Id);

                    grid[gridCoordinate] = 0;
                    _overlappedClaims++;
                }
                else
                {
                    // 2nd, 3rd etc claim overlap
                    elfIds.Remove(elf.Id);
                }
            }
        }

        claimWithoutOverlap = elfIds.Single();
    }
}