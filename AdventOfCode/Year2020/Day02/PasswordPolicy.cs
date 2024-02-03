using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020.Day02;

internal partial class PasswordPolicy
{
    public int FirstNumber { get; private set; }

    public int SecondNumber { get; private set; }

    public char Character { get; private set; }

    public string Password { get; private set; } = string.Empty;

    public static PasswordPolicy Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        var passwordPolicy = new PasswordPolicy
        {
            FirstNumber = int.Parse(match.Groups[1].Value),
            SecondNumber = int.Parse(match.Groups[2].Value),
            Character = match.Groups[3].Value[0],
            Password = match.Groups[4].Value,
        };

        return passwordPolicy;
    }

    [GeneratedRegex("(\\d+)-(\\d+) ([a-z]): ([a-z]+)")]
    private static partial Regex InputLineRegex();
}
