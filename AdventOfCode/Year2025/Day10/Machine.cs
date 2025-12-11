using AdventOfCode.Utilities.Extensions;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2025.Day10;

internal partial class Machine
{
    public int IndicatorLightsBitmask { get; private set; }

    public ButtonWiring[] ButtonWirings { get; private set; } = [];

    public int[] JoltageRequirements { get; private set; } = [];

    public static Machine Parse(string line)
    {
        var matches = InputLineRegex().Matches(line);

        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var indicatorLightsString = matches[0].Groups[1].Value;

        var machine = new Machine
        {
            IndicatorLightsBitmask = indicatorLightsString.Aggregate(0, (agg, curr) => (agg << 1) + (curr == '#' ? 1 : 0)),
            ButtonWirings = [.. matches[0].Groups[2].Value
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(buttons => new ButtonWiring(indicatorLightsString.Length, buttons[1..^1].SplitToNumbers<int>(',')))],
            JoltageRequirements = matches[0].Groups[3].Value.SplitToNumbers<int>(',')
        };

        return machine;
    }

    [GeneratedRegex(@"\[([^\]]*)\]\s*((?:\([^)]*\)\s*)+)\s*\{([^}]*)\}")]
    private static partial Regex InputLineRegex();
}
