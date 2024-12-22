using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2024.Day22;

[Puzzle(2024, 22, "Monkey Market")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private List<long> _secretNubers = [];

    public void ParseInput(string[] inputLines)
    {
        _secretNubers = inputLines.Select(long.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _secretNubers.Sum(x => Enumerable.Range(0, 2000)
                                                      .Aggregate(x, (secret, _) => GenerateSecretNumber(secret)));

        return new PuzzleAnswer(answer, 14082561342L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var totalPatternBananas = new Dictionary<(long, long, long, long), long>();

        foreach (var secretNumber in _secretNubers)
        {
            var seenPatterns = new HashSet<(long, long, long, long)>();

            var prices = Enumerable.Range(0, 2000)
                                   .Aggregate(new List<long> { secretNumber }, (list, _) =>
                                   {
                                       list.Add(GenerateSecretNumber(list[^1]));
                                       return list;
                                   })
                                   .Select(x => x % 10)
                                   .ToList();

            var priceChanges = prices.SlidingPairs()
                                     .Select(x => x.NextItem - x.CurrentItem)
                                     .ToList();

            for (var index = 0; index < priceChanges.Count - 4; index++)
            {
                var pattern = (priceChanges[index], priceChanges[index + 1], priceChanges[index + 2], priceChanges[index + 3]);

                if (seenPatterns.Add(pattern))
                {
                    var price = prices[index + 4];
                    totalPatternBananas[pattern] = totalPatternBananas.GetValueOrDefault(pattern, 0) + price;
                }
            }
        }

        var answer = totalPatternBananas.Max(x => x.Value);

        return new PuzzleAnswer(answer, 1568);
    }

    private static long GenerateSecretNumber(long secret)
    {
        secret = MixAndPrune(secret, secret * 64);
        secret = MixAndPrune(secret, secret / 32);
        secret = MixAndPrune(secret, secret * 2048);

        return secret;
    }

    private static long MixAndPrune(long secret, long result)
    {
        secret = secret ^ result; // Mix
        secret = secret % 16777216; // Prune

        return secret;
    }
}