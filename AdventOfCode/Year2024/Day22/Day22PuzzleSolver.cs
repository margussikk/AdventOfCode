using AdventOfCode.Framework.Puzzle;
using System.Collections.Concurrent;

namespace AdventOfCode.Year2024.Day22;

[Puzzle(2024, 22, "Monkey Market")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private List<long> _secretNumbers = [];

    public void ParseInput(string[] inputLines)
    {
        _secretNumbers = inputLines.Select(long.Parse)
                                   .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = 0L;

        Parallel.ForEach(_secretNumbers, secretNumber =>
        {
            for (var i = 0; i < 2000; i++)
            {
                secretNumber = GenerateSecretNumber(secretNumber);
            }

            Interlocked.Add(ref answer, secretNumber);
        });

        return new PuzzleAnswer(answer, 14082561342L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var totalPatternBananas = new ConcurrentDictionary<long, long>();

        Parallel.ForEach(_secretNumbers, secretNumber =>
        {
            var seenPatterns = new HashSet<long>();

            var prices = new List<long>(2000);
            for (var i = 0; i < 2000; i++)
            {
                secretNumber = GenerateSecretNumber(secretNumber);
                prices.Add(secretNumber % 10);
            }

            var priceChanges = new List<long>(1999);
            for (var i = 1; i < 2000; i++)
            {
                priceChanges.Add(prices[i] - prices[i - 1]);
            }

            for (var index = 0; index < priceChanges.Count - 4; index++)
            {
                var pattern = Hash(priceChanges[index], priceChanges[index + 1], priceChanges[index + 2], priceChanges[index + 3]);

                if (seenPatterns.Add(pattern))
                {
                    var price = prices[index + 4];
                    totalPatternBananas.AddOrUpdate(pattern, price, (key, value) => value + price);
                }
            }
        });

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

    private static long Hash(long priceChange1, long priceChange2, long priceChange3, long priceChange4)
    {
        return ((((((priceChange1 + 9) * 18) + priceChange2 + 9) * 18) + priceChange3 + 9) * 18) + priceChange4 + 9;
    }
}