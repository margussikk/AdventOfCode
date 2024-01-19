using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day03;

[Puzzle(2022, 3, "Rucksack Reorganization")]
public class Day03PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyCollection<Rucksack> _rucksacks = [];

    public void ParseInput(string[] inputLines)
    {
        _rucksacks = inputLines.Select(Rucksack.Parse).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _rucksacks.Select(rucksack => rucksack.Compartment1Items.Intersect(rucksack.Compartment2Items).First())
                               .Sum(CalculateItemPriority);

        return new PuzzleAnswer(answer, 8123);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _rucksacks.Chunk(3)
                .Select(group => group
                    .Select(rucksack => (rucksack.Compartment1Items + rucksack.Compartment2Items).ToCharArray())
                    .Aggregate((accumulator, current) => accumulator.Intersect(current).ToArray())[0])
                .Sum(CalculateItemPriority);

        return new PuzzleAnswer(answer, 2620);
    }

    private static int CalculateItemPriority(char item)
    {
        if (item >= 'a' && item <= 'z')
        {
            return item - 'a' + 1;
        }
        else if (item >= 'A' && item <= 'Z')
        {
            return item - 'A' + 27;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
