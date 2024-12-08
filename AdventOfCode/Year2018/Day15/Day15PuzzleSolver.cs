using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2018.Day15;

[Puzzle(2018, 15, "Beverage Bandits")]
public class Day15PuzzleSolver : IPuzzleSolver
{
    private BitGrid _wallGrid = new(0, 0);

    private readonly List<Unit> _units = [];

    public void ParseInput(string[] inputLines)
    {
        _wallGrid = new BitGrid(inputLines.Length, inputLines[0].Length);

        for (var row = 0; row < inputLines.Length; row++)
        {
            for (var column = 0; column < inputLines[row].Length; column++)
            {
                var symbol = inputLines[row][column];

                if (symbol == '#')
                {
                    _wallGrid[row, column] = true;
                }
                else if (symbol == 'E' || symbol == 'G')
                {
                    var unit = new Unit
                    {
                        UnitType = symbol == 'E' ? UnitType.Elf : UnitType.Goblin,
                        Coordinate = new GridCoordinate(row, column),
                        AttackPower = 3,
                    };

                    _units.Add(unit);
                }
            }
        }
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

                attackPowerRange = new NumberRange<int>(attackPowerRange.Start, elfAttackPower);
            }
            else
            {
                attackPowerRange = new NumberRange<int>(elfAttackPower + 1, attackPowerRange.End);
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
                var isTargetAttackable = currentUnit.Coordinate
                    .SideNeighbors()
                    .Intersect(targets.Select(t => t.Coordinate))
                    .Any();

                if (!isTargetAttackable)
                {
                    var occupiedCoordinates = units.Select(u => u.Coordinate).ToHashSet();

                    var walkerResults = targets.SelectMany(target => _wallGrid.SideNeighbors(target.Coordinate))
                                               .DistinctBy(cell => cell.Coordinate)
                                               .Where(cell => !_wallGrid[cell.Coordinate] && !occupiedCoordinates.Contains(cell.Coordinate))
                                               .Select(cell => FindShortestDistance(currentUnit.Coordinate, cell.Coordinate, occupiedCoordinates))
                                               .Where(result => result.Distance != int.MaxValue)
                                               .ToList();

                    if (walkerResults.Count != 0)
                    {
                        var shortestDistance = walkerResults.Min(result => result.Distance);

                        currentUnit.Coordinate = walkerResults
                            .Where(result => result.Distance == shortestDistance)
                            .Select(result => FindBestPath(currentUnit.Coordinate, result.Coordinate, occupiedCoordinates))
                            .OrderBy(result => result.Coordinate.Row)
                            .ThenBy(result => result.Coordinate.Column)
                            .Select(result => result.Path[1]) // [0] is the start coordinate, [1] is the next coordinate
                            .First();
                    }
                }

                // Attack
                var targetToAttack = targets.Where(target => currentUnit.Coordinate.SideNeighbors().Contains(target.Coordinate))
                                            .OrderBy(unit => unit.HitPoints)
                                            .ThenBy(unit => unit.Coordinate.Row)
                                            .ThenBy(unit => unit.Coordinate.Column)
                                            .FirstOrDefault();

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

    // Find shortest distance using Dijkstra
    private WalkerResult FindShortestDistance(GridCoordinate startCoordinate, GridCoordinate endCoordinate, HashSet<GridCoordinate> occupiedCoordinates)
    {
        var shortestDistancesGrid = new Grid<int?>(_wallGrid.Height, _wallGrid.Width);

        var walkerQueue = new PriorityQueue<GridWalker, int>();
        var walker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);
        walkerQueue.Enqueue(walker, walker.Steps);

        while (walkerQueue.TryDequeue(out walker, out _))
        {
            if (walker.CurrentCoordinate == endCoordinate)
            {
                return new WalkerResult(endCoordinate, walker.Steps, []);
            }

            if (shortestDistancesGrid[walker.CurrentCoordinate].HasValue)
            {
                continue;
            }

            shortestDistancesGrid[walker.CurrentCoordinate] = walker.Steps;

            foreach (var neighborCell in _wallGrid.SideNeighbors(walker.CurrentCoordinate))
            {
                if (neighborCell.Object || occupiedCoordinates.Contains(neighborCell.Coordinate))
                {
                    continue;
                }

                var currentDistance = shortestDistancesGrid[neighborCell.Coordinate] ?? int.MaxValue;
                if (walker.Steps + 1 >= currentDistance) continue;

                var direction = walker.CurrentCoordinate.DirectionToward(neighborCell.Coordinate);

                var newWalker = walker.Clone();

                newWalker.Move(direction);
                walkerQueue.Enqueue(newWalker, newWalker.Steps);
            }
        }

        return new WalkerResult(endCoordinate, int.MaxValue, []);
    }

    // Find the "best" path using Dijkstra
    private WalkerResult FindBestPath(GridCoordinate startCoordinate, GridCoordinate endCoordinate, HashSet<GridCoordinate> occupiedCoordinates)
    {
        int shortestDistance = int.MaxValue;
        var shortestDistancesGrid = new Grid<int?>(_wallGrid.Height, _wallGrid.Width);
        var bestPaths = new Grid<List<GridCoordinate>>(_wallGrid.Height, _wallGrid.Width);

        var walkerQueue = new PriorityQueue<GridWalker, int>();
        var walker = new GridWalker(startCoordinate, startCoordinate, GridDirection.None, 0);
        walkerQueue.Enqueue(walker, walker.Steps);

        while (walkerQueue.TryDequeue(out walker, out _))
        {
            walker.Breadcrumbs.Add(walker.CurrentCoordinate);

            if (walker.CurrentCoordinate == endCoordinate)
            {
                shortestDistance = walker.Steps;

                if (bestPaths[walker.CurrentCoordinate] == null || !IsCurrentPathBetter(walker))
                {
                    bestPaths[walker.CurrentCoordinate] = walker.Breadcrumbs;
                }

                continue;
            }

            if (shortestDistancesGrid[walker.CurrentCoordinate].HasValue)
            {
                if (shortestDistancesGrid[walker.CurrentCoordinate] != walker.Steps)
                {
                    continue;
                }
                
                if (IsCurrentPathBetter(walker))
                {
                    continue;
                }
            }

            if (walker.Steps > shortestDistance)
            {
                continue;
            }

            shortestDistancesGrid[walker.CurrentCoordinate] = walker.Steps;
            bestPaths[walker.CurrentCoordinate] = walker.Breadcrumbs;

            foreach (var neighborCell in _wallGrid.SideNeighbors(walker.CurrentCoordinate))
            {
                if (neighborCell.Object || occupiedCoordinates.Contains(neighborCell.Coordinate))
                {
                    continue;
                }

                var currentDistance = shortestDistancesGrid[neighborCell.Coordinate] ?? int.MaxValue;
                if (walker.Steps + 1 >= currentDistance) continue;

                var direction = walker.CurrentCoordinate.DirectionToward(neighborCell.Coordinate);

                var newWalker = walker.Clone();
                newWalker.Breadcrumbs.AddRange(walker.Breadcrumbs);                

                newWalker.Move(direction);
                walkerQueue.Enqueue(newWalker, newWalker.Steps);
            }
        }

        if (shortestDistance == int.MaxValue)
        {
            return new WalkerResult(endCoordinate, int.MaxValue, []);
        }

        return new WalkerResult(endCoordinate, shortestDistance, bestPaths[endCoordinate]);


        bool IsCurrentPathBetter(GridWalker walker)
        {
            var currentBestPath = bestPaths[walker.CurrentCoordinate];

            for (var i = 0; i < currentBestPath.Count; i++)
            {
                var compare = currentBestPath[i].Row.CompareTo(walker.Breadcrumbs[i].Row);
                if (compare == 0)
                {
                    compare = currentBestPath[i].Column.CompareTo(walker.Breadcrumbs[i].Column);
                }

                if (compare != 0)
                {
                    return compare < 0;
                }
            }

            return true;
        }
    }

    private sealed record WalkerResult(GridCoordinate Coordinate, int Distance, List<GridCoordinate> Path);

    private sealed record CombatOutcome(UnitType Winner, int Round, int TotalHitPointsLeft);
}