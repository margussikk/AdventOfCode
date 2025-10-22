using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day17;

[Puzzle(2015, 17, "No Such Thing as Too Much")]
public class Day17PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<int> _containerSizes = [];

    public void ParseInput(string[] inputLines)
    {
        _containerSizes = [.. inputLines.Select(int.Parse).Order()];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var allCombinations = new List<List<int>>();

        FindCombinations(150, 0, [], allCombinations);

        var answer = allCombinations.Count;

        return new PuzzleAnswer(answer, 654);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var allCombinations = new List<List<int>>();

        FindCombinations(150, 0, [], allCombinations);

        var containerCount = allCombinations
            .GroupBy(x => x.Count)
            .ToDictionary(x => x.Key, x => x.Count());

        var answer = containerCount.MinBy(x => x.Key).Value;

        return new PuzzleAnswer(answer, 57);
    }

    private void FindCombinations(int targetVolume, int index, List<int> currentCombination, List<List<int>> allCombinations)
    {
        if (targetVolume == 0)
        {
            allCombinations.Add(currentCombination);
            return;
        }

        if (targetVolume < 0 || index >= _containerSizes.Count)
        {
            return;
        }

        FindCombinations(targetVolume, index + 1, currentCombination, allCombinations);
        FindCombinations(targetVolume - _containerSizes[index], index + 1, [.. currentCombination, _containerSizes[index]], allCombinations);
    }
}