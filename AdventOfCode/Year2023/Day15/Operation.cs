using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023.Day15;

internal abstract partial class Operation
{
    public string Label { get; }

    public int GetLabelHash() => GetHash(Label);

    public int GetHash() => GetHash(ToString()!);

    protected Operation(string label)
    {
        Label = label;
    }
    
    public static Operation Parse(string input)
    {
        var matches = InputLineRegex().Matches(input);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException();
        }

        switch (matches[0].Groups[2].Value)
        {
            case "=":
            {
                var label = matches[0].Groups[1].Value;
                var focalLength = int.Parse(matches[0].Groups[3].Value);

                return new ReplaceLensOperation(label, focalLength);
            }
            case "-":
            {
                var label = matches[0].Groups[1].Value;

                return new RemoveLensOperation(label);
            }
            default:
                throw new InvalidOperationException();
        }
    }

    private static int GetHash(string input)
    {
        var hash = 0;

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
