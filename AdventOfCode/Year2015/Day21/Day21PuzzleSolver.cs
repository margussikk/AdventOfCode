using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2015.Day21;

[Puzzle(2015, 21, "RPG Simulator 20XX")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private static readonly IReadOnlyList<Item> _weapons =
    [
        new("Dagger",        8, 4, 0),
        new("Shortsword",   10, 5, 0),
        new("Warhammer",    25, 6, 0),
        new("Longsword",    40, 7, 0),
        new("Greataxe",     74, 8, 0),
    ];

    private static readonly IReadOnlyList<Item> _armors =
    [
        new("Leather",      13, 0, 1),
        new("Chainmail",    31, 0, 2),
        new("Splintmail",   53, 0, 3),
        new("Bandedmail",   75, 0, 4),
        new("Platemail",   102, 0, 5),
    ];

    private static readonly IReadOnlyList<Item> _rings =
    [
        new("Damage +1",    25, 1, 0),
        new("Damage +2",    50, 2, 0),
        new("Damage +3",   100, 3, 0),
        new("Defense +1",   20, 0, 1),
        new("Defense +2",   40, 0, 2),
        new("Defense +3",   80, 0, 3),
    ];

    private Character _boss = null!;

    public void ParseInput(string[] inputLines)
    {
        _boss = Character.Parse(inputLines);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = int.MaxValue;

        foreach (var items in GetItems())
        {
            var totalCost = items.Sum(i => i.Cost);
            var totalDamage = items.Sum(i => i.Damage);
            var totalArmor = items.Sum(i => i.Armor);

            var you = new Character(100, totalDamage, totalArmor);
            if (SimulateBattle(you, _boss.Clone()))
            {
                answer = int.Min(answer, totalCost);
            }
        }

        return new PuzzleAnswer(answer, 78);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = int.MinValue;

        foreach (var items in GetItems())
        {
            var totalCost = items.Sum(i => i.Cost);
            var totalDamage = items.Sum(i => i.Damage);
            var totalArmor = items.Sum(i => i.Armor);

            var you = new Character(100, totalDamage, totalArmor);
            if (!SimulateBattle(you, _boss.Clone()))
            {
                answer = int.Max(answer, totalCost);
            }
        }

        return new PuzzleAnswer(answer, 148);
    }

    private static IEnumerable<IEnumerable<Item>> GetItems()
    {
        foreach (var weapon in _weapons.Select(MapToItemArray))
        {
            foreach (var armor in _armors.Select(MapToItemArray).Concat([[]]))
            {
                foreach (var rings in _rings.Select(MapToItemArray).Concat(_rings.GetCombinations(2)).Concat([]))
                {
                    yield return [.. weapon, .. armor, .. rings];
                }
            }
        }

        static Item[] MapToItemArray(Item x) => [x];
    }

    private static bool SimulateBattle(Character attacker, Character defender)
    {
        var boss = defender;

        while (true)
        {
            var damageDealt = Math.Max(1, attacker.Damage - defender.Armor);
            defender.HitPoints -= damageDealt;
            if (defender.HitPoints <= 0)
            {
                return defender == boss;
            }
            (attacker, defender) = (defender, attacker);
        }
    }
}