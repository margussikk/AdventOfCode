using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015.Day15;

internal partial class Ingredient
{
    public string Name { get; private init; } = string.Empty;
    public int Capacity { get; private init; }
    public int Durability { get; private init; }
    public int Flavor { get; private init; }
    public int Texture { get; private init; }
    public int Calories { get; private init; }

    public static Ingredient Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        return new Ingredient
        {
            Name = match.Groups[1].Value,
            Capacity = int.Parse(match.Groups[2].Value),
            Durability = int.Parse(match.Groups[3].Value),
            Flavor = int.Parse(match.Groups[4].Value),
            Texture = int.Parse(match.Groups[5].Value),
            Calories = int.Parse(match.Groups[6].Value)
        };
    }

    [GeneratedRegex(@"(\w+): capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)")]
    private static partial Regex InputLineRegex();
}
