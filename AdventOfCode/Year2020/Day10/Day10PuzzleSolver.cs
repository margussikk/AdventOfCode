using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day10;

[Puzzle(2020, 10, "Adapter Array")]
public class Day10PuzzleSolver : IPuzzleSolver
{
    private List<int> _adapterJoltages = [];

    public void ParseInput(string[] inputLines)
    {
        _adapterJoltages = inputLines.Select(int.Parse)
                                     .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var joltageDifferences = new int[3];
        List<int> joltages = [0, .. _adapterJoltages.Order()];

        joltageDifferences[2]++; // Built-in is always 3 higher

        for (var index = 0; index < joltages.Count - 1; index++)
        {
            var difference = joltages[index + 1] - joltages[index];
            joltageDifferences[difference - 1]++;
        }

        var answer = joltageDifferences[0] * joltageDifferences[2];

        return new PuzzleAnswer(answer, 2312);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var cache = new Dictionary<int, long>();
        List<int> joltages = [0, .. _adapterJoltages.Order()];

        var answer = Count(0);

        return new PuzzleAnswer(answer, 12089663946752L);


        long Count(int currentIndex)
        {
            if (currentIndex == joltages.Count - 1)
            {
                return 1L;
            }

            if (cache.TryGetValue(currentIndex, out var count))
            {
                return count;
            }

            for (var nextIndex = currentIndex + 1;
                nextIndex < joltages.Count && joltages[nextIndex] - joltages[currentIndex] <= 3;
                nextIndex++)
            {
                count += Count(nextIndex);
            }

            cache[currentIndex] = count;

            return count;
        }
    }
}