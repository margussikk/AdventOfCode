using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2018.Day24;

[Puzzle(2018, 24, "Immune System Simulator 20XX")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private readonly List<Group> _groups = [];

    public void ParseInput(string[] inputLines)
    {
        foreach (var chunk in inputLines.SelectToChunks())
        {
            var armyType = chunk[0] switch
            {
                "Immune System:" => ArmyType.ImmuneSystem,
                "Infection:" => ArmyType.Infection,
                _ => throw new InvalidOperationException("Invalid army name")
            };

            _groups.AddRange(chunk.Skip(1)
                                  .Select((input, index) => Group.Parse(armyType, index + 1, input)));
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var fightResult = Fight(0);

        return new PuzzleAnswer(fightResult.UnitsLeft, 17738);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var numberRange = new NumberRange<int>(0, 64);

        var fightResult = new FightResult(ArmyType.Infection, 0);

        while (numberRange.Length > 1)
        {
            var boost = (numberRange.Start + numberRange.End) / 2;

            fightResult = Fight(boost);
            if (fightResult.Winner == ArmyType.ImmuneSystem)
            {
                numberRange = numberRange.SplitAfter(boost)[0];
            }
            else
            {
                numberRange = numberRange.SplitAfter(boost)[1];
            }
        }

        return new PuzzleAnswer(fightResult.UnitsLeft, 3057);
    }

    private FightResult Fight(int immuneSystemBoost)
    {
        var groups = _groups
            .Select(g =>
            {
                var newGroup = g.Clone();
                newGroup.Boost = newGroup.ArmyType == ArmyType.ImmuneSystem
                    ? immuneSystemBoost
                    : 0;

                return newGroup;
            })
            .ToList();

        while (groups.Select(g => g.ArmyType).Distinct().Count() > 1)
        {
            var unitsGotKilled = false;

            // Target selection phase
            foreach (var group in groups)
            {
                group.TargetGroupId = default;
                group.IsTargeted = false;
            }

            foreach (var targetingGroup in groups.OrderByDescending(g => g.EffectivePower)
                                                 .ThenByDescending(x => x.Initiative))
            {
                var defendingGroup = groups
                    .Where(g => g.ArmyType != targetingGroup.ArmyType && !g.IsTargeted)
                    .OrderByDescending(g => g.EffectiveDamageTakenFrom(targetingGroup))
                    .ThenByDescending(g => g.EffectivePower)
                    .ThenByDescending(g => g.Initiative)
                    .FirstOrDefault();

                if (defendingGroup != null && defendingGroup.EffectiveDamageTakenFrom(targetingGroup) > 0)
                {
                    targetingGroup.TargetGroupId = defendingGroup.GroupId;
                    defendingGroup.IsTargeted = true;
                }
            }

            // Attacking phase
            foreach (var attackingGroup in groups.Where(g => g.TargetGroupId.HasValue)
                                                 .OrderByDescending(g => g.Initiative))
            {
                var defendingGroup = groups.FirstOrDefault(g => g.ArmyType != attackingGroup.ArmyType && g.GroupId == attackingGroup.TargetGroupId);
                if (defendingGroup == null)
                {
                    continue;
                }

                var unitsKilled = Math.Min(defendingGroup.Units, defendingGroup.EffectiveDamageTakenFrom(attackingGroup) / defendingGroup.HitPoints);
                if (unitsKilled > 0)
                {
                    unitsGotKilled = true;
                }

                defendingGroup.Units -= unitsKilled;
            }

            if (unitsGotKilled)
            {
                groups = groups.Where(g => g.Units > 0).ToList();
            }
            else
            {
                // No units got killed, so to break the infinite loop, let's just make infection army win
                groups = groups.Where(g => g.ArmyType == ArmyType.Infection).ToList();
            }
        }

        return new FightResult(groups[0].ArmyType, groups.Sum(g => g.Units));
    }

    private sealed record FightResult(ArmyType Winner, int UnitsLeft);
}