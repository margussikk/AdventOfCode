using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2015.Day22;

[Puzzle(2015, 22, "Wizard Simulator 20XX")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private static readonly Dictionary<SpellType, SpellInfo> _spells = new()
    {
        [SpellType.MagicMissile] = new SpellInfo(53, 4, 0, 0, 0, 0),
        [SpellType.Drain] = new SpellInfo(73, 2, 0, 2, 0, 0),
        [SpellType.Shield] = new SpellInfo(113, 0, 7, 0, 0, 6),
        [SpellType.Poison] = new SpellInfo(173, 3, 0, 0, 0, 6),
        [SpellType.Recharge] = new SpellInfo(229, 0, 0, 0, 101, 5)
    };

    private Boss _boss = null!;

    public void ParseInput(string[] inputLines)
    {
        _boss = Boss.Parse(inputLines);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(false);

        return new PuzzleAnswer(answer, 1824);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(true);

        return new PuzzleAnswer(answer, 1937);
    }

    private int GetAnswer(bool hardMode)
    {
        var wizard = new Player
        {
            HitPoints = 50,
            Mana = 500,
        };

        var queue = new PriorityQueue<State, int>();
        queue.Enqueue(new State { Player = wizard, Boss = _boss }, 0);

        while (queue.TryDequeue(out var state, out _))
        {
            if (state.Boss.HitPoints <= 0)
            {
                return state.Player.SpentMana;
            }

            foreach (var spellType in _spells.Keys)
            {
                var player = state.Player.Clone();
                var boss = state.Boss.Clone();

                var continueFighting = Fight(player, boss, spellType, hardMode);
                if (continueFighting)
                {
                    var newState = new State
                    {
                        Player = player,
                        Boss = boss
                    };

                    queue.Enqueue(newState, player.SpentMana);
                }
            }
        }

        return 0;
    }

    private static bool Fight(Player player, Boss boss, SpellType spellType, bool hardMode)
    {
        // Player's turn
        if (hardMode)
        {
            player.HitPoints--;
            if (player.HitPoints <= 0)
            {
                return false;
            }
        }

        ApplyEffects(player, boss);

        if (boss.HitPoints <= 0)
        {
            return true;
        }

        var chosenSpell = _spells[spellType];
        if (chosenSpell.Cost > player.Mana || player.ActiveEffects.ContainsKey(spellType))
        {
            return false;
        }

        player.Mana -= chosenSpell.Cost;
        player.SpentMana += chosenSpell.Cost;
        if (chosenSpell.IsInstant)
        {
            chosenSpell.ApplyEffects(player, boss);

            if (boss.HitPoints <= 0)
            {
                return true;
            }
        }
        else
        {
            player.ActiveEffects[spellType] = chosenSpell.EffectTurns;
        }

        // Boss's turn
        ApplyEffects(player, boss);

        if (boss.HitPoints <= 0)
        {
            return true;
        }

        var playerArmor = player.ActiveEffects.Sum(kvp => _spells[kvp.Key].Armor);
        player.HitPoints -= Math.Max(1, boss.Damage - playerArmor);

        return player.HitPoints > 0;
    }

    private static void ApplyEffects(Player player, Boss boss)
    {
        foreach (var effectiveSpell in player.ActiveEffects.ToList())
        {
            _spells[effectiveSpell.Key].ApplyEffects(player, boss);

            if (effectiveSpell.Value == 1)
            {
                player.ActiveEffects.Remove(effectiveSpell.Key);
            }
            else
            {
                player.ActiveEffects.IncrementValue(effectiveSpell.Key, -1);
            }
        }
    }
}