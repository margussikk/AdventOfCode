using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022.Day19;

internal partial class Blueprint
{
    public int Id { get; private set; }

    public int OreRobotCostOre { get; private set; }
    public int ClayRobotCostOre { get; private set; }
    public int ObsidianRobotCostOre { get; private set; }
    public int ObsidianRobotCostClay { get; private set; }
    public int GeodeRobotCostOre { get; private set; }
    public int GeodeRobotCostObsidian { get; private set; }

    public int MaxOreNeededPerMinute { get; set; }

    public static Blueprint Parse(string line)
    {
        var matches = LineRegex().Matches(line);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Parsing blueprint failed");
        }

        var match = matches[0];

        var blueprint = new Blueprint()
        {
            Id = int.Parse(match.Groups[1].Value),
            OreRobotCostOre = int.Parse(match.Groups[2].Value),
            ClayRobotCostOre = int.Parse(match.Groups[3].Value),
            ObsidianRobotCostOre = int.Parse(match.Groups[4].Value),
            ObsidianRobotCostClay = int.Parse(match.Groups[5].Value),
            GeodeRobotCostOre = int.Parse(match.Groups[6].Value),
            GeodeRobotCostObsidian = int.Parse(match.Groups[7].Value),
        };

        blueprint.MaxOreNeededPerMinute = blueprint.OreRobotCostOre;
        blueprint.MaxOreNeededPerMinute = Math.Max(blueprint.MaxOreNeededPerMinute, blueprint.ClayRobotCostOre);
        blueprint.MaxOreNeededPerMinute = Math.Max(blueprint.MaxOreNeededPerMinute, blueprint.ObsidianRobotCostClay);
        blueprint.MaxOreNeededPerMinute = Math.Max(blueprint.MaxOreNeededPerMinute, blueprint.GeodeRobotCostOre);

        return blueprint;
    }

    [GeneratedRegex("Blueprint (\\d+): Each ore robot costs (\\d+) ore. Each clay robot costs (\\d+) ore. Each obsidian robot costs (\\d+) ore and (\\d+) clay. Each geode robot costs (\\d+) ore and (\\d+) obsidian.")]
    private static partial Regex LineRegex();
}
