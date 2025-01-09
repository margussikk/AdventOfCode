using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018.Day24;
internal partial class Group
{
    public ArmyType ArmyType { get; private set; }
    public int GroupId { get; private set; }

    public int? TargetGroupId { get; set; }
    public bool IsTargeted = false;

    public int Units { get; set; }
    public int HitPoints { get; set; }
    public int AttackDamage { get; private set; }
    public int Boost { get; set; }

    public string AttackType { get; private set; } = string.Empty;
    public int Initiative { get; private set; }

    public List<string> Weaknesses { get; private set; } = [];
    public List<string> Immunities { get; private set; } = [];

    public int EffectivePower => Units * (AttackDamage + Boost);

    public Group Clone()
    {
        return new Group
        {
            ArmyType = ArmyType,
            GroupId = GroupId,
            TargetGroupId = TargetGroupId,
            IsTargeted = IsTargeted,
            Units = Units,
            HitPoints = HitPoints,
            AttackDamage = AttackDamage,
            Boost = Boost,
            AttackType = AttackType,
            Initiative = Initiative,
            Weaknesses = [.. Weaknesses],
            Immunities = [.. Immunities]
        };
    }

    public int EffectiveDamageTakenFrom(Group attackingGroup)
    {
        if (Immunities.Contains(attackingGroup.AttackType))
        {
            return 0;
        }

        if (Weaknesses.Contains(attackingGroup.AttackType))
        {
            return attackingGroup.EffectivePower * 2;
        }

        return attackingGroup.EffectivePower;
    }

    public static Group Parse(ArmyType armyType, int groupId, string inputLine)
    {
        var group = new Group
        {
            ArmyType = armyType,
            GroupId = groupId,
        };

        var matches = InputLineRegex().Matches(inputLine);
        if (matches.Count != 1)
        {
            throw new InvalidOperationException("Failed to parse input line");
        }

        var match = matches[0];

        group.Units = int.Parse(match.Groups[1].Value);
        group.HitPoints = int.Parse(match.Groups[2].Value);

        if (match.Groups[3].Value.Length != 0)
        {
            const string weakTo = "weak to";
            const string immuneTo = "immune to";

            var splits = match.Groups[3].Value[1..^1].Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var split in splits)
            {
                if (split[0..weakTo.Length] == weakTo)
                {
                    group.Weaknesses = [.. split[weakTo.Length..].Split(", ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
                }
                else if (split[0..immuneTo.Length] == immuneTo)
                {
                    group.Immunities = [.. split[immuneTo.Length..].Split(", ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected relation to damage type: {split}");
                }
            }
        }

        group.AttackDamage = int.Parse(match.Groups[4].Value);
        group.AttackType = match.Groups[5].Value;
        group.Initiative = int.Parse(match.Groups[6].Value);

        return group;
    }

    [GeneratedRegex(@"(\d+) units each with (\d+) hit points (\(.+\))? ?with an attack that does (\d+) (\w+) damage at initiative (\d+)")]
    private static partial Regex InputLineRegex();
}
