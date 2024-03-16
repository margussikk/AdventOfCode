using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023.Day15;

internal abstract partial class Operation(string label)
{
    public string Label { get; private set; } = label;

    public int GetLabelHash() => GetHash(Label);

    public int GetHash() => GetHash(ToString()!);

    public static Operation Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException();
        }

        if (matches[0].Groups[2].Value == "=")
        {
            var label1 = matches[0].Groups[1].Value;
            var focalLength = int.Parse(matches[0].Groups[3].Value);

            return new ReplaceLensOperation(label1, focalLength);
        }
        else if (matches[0].Groups[2].Value == "-")
        {
            var label1 = matches[0].Groups[1].Value;

            return new RemoveLensOperation(label1);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private static int GetHash(string input)
    {
        int hash = 0;

        foreach (var character in input)
        {
            hash += character;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }

    [GeneratedRegex("([a-z]+)([=-])([\\d]?)")]
    private static partial Regex InputLineRegex();
}
