using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;
using AdventOfCode.Utilities.Numerics;
using AdventOfCode.Utilities.PathFinding;

namespace AdventOfCode.Year2018.Day15;

[Puzzle(2018, 15, "Beverage Bandits")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private Grid<bool> _wallGrid = new(0, 0);

    private readonly List<Unit> _units = [];

    public void ParseInput(string[] inputLines)
    {
        _wallGrid = inputLines.SelectToGrid((character, coordinate) =>
        {
            if (character == 'E' || character == 'G')
            {
                var unit = new Unit
                {
                    UnitType = character == 'E' ? UnitType.Elf : UnitType.Goblin,
                    Coordinate = coordinate,
                    AttackPower = 3,
                };

                _units.Add(unit);
            }

            return character == '#';
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var combatOutcome = DoCombat(3, true);

        var answer = combatOutcome.Round * combatOutcome.TotalHitPointsLeft;
        return new PuzzleAnswer(answer, 229950);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var attackPowerRange = new NumberRange<int>(4, 64);

        while (true)
        {
            var elfAttackPower = (attackPowerRange.Start + attackPowerRange.End) / 2;

            var combatOutcome = DoCombat(elfAttackPower, false);
            if (combatOutcome.Winner == UnitType.Elf)
            {
                if (attackPowerRange.Length == 1)
                {
                    var answer = combatOutcome.Round * combatOutcome.TotalHitPointsLeft;
                    return new PuzzleAnswer(answer, 54360);
                }

                attackPowerRange = attackPowerRange.SplitAfter(elfAttackPower)[0];
            }
            else
            {
                attackPowerRange = attackPowerRange.SplitAfter(elfAttackPower)[1];
            }
        }
    }

    private CombatOutcome DoCombat(int elfAttackPower, bool elfCanDie)
    {
        var units = _units
            .Select(u => new Unit
            {
                UnitType = u.UnitType,
                Coordinate = u.Coordinate,
                HitPoints = 200,
                AttackPower = u.UnitType == UnitType.Elf ? elfAttackPower : u.AttackPower,
            })
            .ToList();

        for (var round = 1; round <= int.MaxValue; round++)
        {
            units.ForEach(u => u.Acted = false);

            units = [.. units.OrderBy(u => u.Coordinate.Row)
                             .ThenBy(u => u.Coordinate.Column)];

            while (units.Any(u => !u.Acted))
            {
                var currentUnit = units.First(u => !u.Acted);

                var targetUnitType = currentUnit.UnitType == UnitType.Elf ? UnitType.Goblin : UnitType.Elf;
                var targets = units.Where(u => u.UnitType == targetUnitType)
                                   .ToList();

                if (targets.Count == 0)
                {
                    // Combat ended
                    return new CombatOutcome(currentUnit.UnitType, round - 1, units.Sum(u => u.HitPoints));
                }

                // Move
                var targetToAttack = targets.Where(target => currentUnit.Coordinate.SideNeighbors().Contains(target.Coordinate))
                                            .OrderBy(unit => unit.HitPoints)
                                            .ThenBy(unit => unit.Coordinate.Row)
                                            .ThenBy(unit => unit.Coordinate.Column)
                                            .FirstOrDefault();
                if (targetToAttack == null)
                {
                    var occupiedCoordinates = units.Select(u => u.Coordinate).ToHashSet();

                    var endCoordinates = targets.SelectMany(target => target.Coordinate.SideNeighbors()
                                                .Where(coordinate => _wallGrid.InBounds(coordinate) && !_wallGrid[coordinate]))
                                                .ToHashSet();

                    var pathFinder = new GridPathFinder<bool>(_wallGrid)
                        .SetCellFilter((walker, cell) => !cell.Object && !occupiedCoordinates.Contains(cell.Coordinate));

                    var paths = pathFinder.FindShortestBestPaths(currentUnit.Coordinate, endCoordinates);
                    if (paths.Count > 0)
                    {
                        var bestPath = paths.OrderBy(x => x.Key.Row)
                                            .ThenBy(x => x.Key.Column)
                                            .Select(x => x.Value)
                                            .First();

                        currentUnit.Coordinate = bestPath[1]; // [0] is the start coordinate, [1] is the next coordinate

                        targetToAttack = targets.Where(target => currentUnit.Coordinate.SideNeighbors().Contains(target.Coordinate))
                                                .OrderBy(unit => unit.HitPoints)
                                                .ThenBy(unit => unit.Coordinate.Row)
                                                .ThenBy(unit => unit.Coordinate.Column)
                                                .FirstOrDefault();
                    }
                }

                // Attack
                if (targetToAttack != null)
                {
                    targetToAttack.HitPoints -= currentUnit.AttackPower;

                    if (targetToAttack.HitPoints <= 0)
                    {
                        if (targetToAttack.UnitType == UnitType.Elf && !elfCanDie)
                        {
                            return new CombatOutcome(UnitType.Goblin, round - 1, units.Sum(u => u.HitPoints));
                        }

                        units.Remove(targetToAttack);
                    }
                }

                // End of turn
                currentUnit.Acted = true;
            }
        }

        return new CombatOutcome(UnitType.Elf, 0, 0);
    }

    private sealed record CombatOutcome(UnitType Winner, int Round, int TotalHitPointsLeft);
}