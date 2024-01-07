namespace AdventOfCode.Year2023.Day04;

internal class Card
{
    public int Id { get; private set; }

    public IReadOnlyList<int> WinningNumbers { get; private set; } = new List<int>();

    public IReadOnlyList<int> YourNumbers { get; private set; } = new List<int>();

    internal static readonly string[] _splitSeparator = [": ", "|"];

    public int CountMatches()
    {
        return WinningNumbers.Intersect(YourNumbers).Count();
    }

    public static Card Parse(string input)
    {
        var splits = input.Split(_splitSeparator, StringSplitOptions.RemoveEmptyEntries);

        var card = new Card()
        {
            Id = int.Parse(splits[0]["Card ".Length..]),
            WinningNumbers = splits[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(int.Parse)
                                      .ToList(),
            YourNumbers = splits[2].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(int.Parse)
                                   .ToList()
        };

        return card;
    }
}
