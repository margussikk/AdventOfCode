using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2023.Day04;

internal class Card
{
    public int Id { get; private set; }

    public IReadOnlyList<int> WinningNumbers { get; private init; } = [];

    public IReadOnlyList<int> YourNumbers { get; private init; } = [];

    private static readonly string[] _splitSeparator = [": ", "|"];

    public int CountMatches()
    {
        return WinningNumbers.Intersect(YourNumbers).Count();
    }

    public static Card Parse(string input)
    {
        var splits = input.Split(_splitSeparator, StringSplitOptions.RemoveEmptyEntries);

        var card = new Card
        {
            Id = int.Parse(splits[0]["Card ".Length..]),
            WinningNumbers = [.. splits[1].SplitToNumbers<int>(' ')],
            YourNumbers = [.. splits[2].SplitToNumbers<int>(' ')]
        };

        return card;
    }
}
