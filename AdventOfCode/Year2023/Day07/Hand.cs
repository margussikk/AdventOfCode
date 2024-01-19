namespace AdventOfCode.Year2023.Day07;

internal class Hand
{
    public char[] Cards { get; private set; } = [];

    public long Bid { get; private set; }

    public HandStrength HandStrength { get; private set; }

    public void FillHandTypeForPartOne()
    {
        var countsList = Cards
            .ToLookup(x => x)
            .Select(x => x.Count())
            .Order()
            .ToList();

        HandStrength = GetHandStrength(countsList);
    }

    public void FillHandTypeForPartTwo()
    {
        var countsList = Cards
            .Where(x => x != 'J') // Jacks are jokers
            .ToLookup(x => x)
            .Select(x => x.Count())
            .Order()
            .ToList();

        var jokerCount = 5 - countsList.Sum();
        if (jokerCount == 5)
        {
            HandStrength = HandStrength.FiveOfAKind;
        }
        else
        {
            // Add jokers to the highest cards group (high card, pair, three of a kind, four of a kind)
            countsList[^1] += jokerCount;

            HandStrength = GetHandStrength(countsList);
        }
    }

    public static Hand Parse(string input)
    {
        var splits = input.Split(' ');

        return new Hand
        {
            Cards = [.. splits[0]],
            Bid = long.Parse(splits[1]),
            HandStrength = HandStrength.HighCard
        };
    }

    private static HandStrength GetHandStrength(List<int> countsList)
    {
        return countsList switch
        {
            [5] => HandStrength.FiveOfAKind,
            [1, 4] => HandStrength.FourOfAKind,
            [2, 3] => HandStrength.FullHouse,
            [1, 1, 3] => HandStrength.ThreeOfAKind,
            [1, 2, 2] => HandStrength.TwoPair,
            [1, 1, 1, 2] => HandStrength.OnePair,
            _ => HandStrength.HighCard
        };
    }
}
