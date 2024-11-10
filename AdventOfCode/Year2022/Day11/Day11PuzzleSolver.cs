using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Year2022.Day11;

[Puzzle(2022, 11, "Monkey in the Middle")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private List<Monkey> _monkeys = [];

    public void ParseInput(string[] inputLines)
    {
        _monkeys = inputLines.SelectToChunks()
                             .Select(Monkey.Parse)
                             .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        foreach (var monkey in _monkeys)
        {
            monkey.Reset();
        }

        var answer = GetMonkeyBusiness(20, x => x / 3);

        return new PuzzleAnswer(answer, 88208);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        foreach (var monkey in _monkeys)
        {
            monkey.Reset();
        }

        var lcm = _monkeys.Select(m => m.TestDivisor).LeastCommonMultiple();
        var answer = GetMonkeyBusiness(10_000, x => x % lcm);

        return new PuzzleAnswer(answer, 21115867968L);
    }

    private long GetMonkeyBusiness(int roundCount, Func<long, long> updateWorryLevel)
    {
        for (var round = 1; round <= roundCount; round++)
        {
            foreach (var monkey in _monkeys)
            {
                while (monkey.Items.Count != 0)
                {
                    // Monkey inspects
                    var worryLevel = monkey.TakeOutInspectedItem();

                    // Change worry level
                    worryLevel = monkey.CalculateWorryLevel(worryLevel);

                    // Monkey gets bored with item
                    worryLevel = updateWorryLevel(worryLevel);

                    // Test and throw
                    var catcher = monkey.DecideCatcher(worryLevel);
                    _monkeys[catcher].CatchItem(worryLevel);
                }
            }
        }

        var monkeyBusiness = _monkeys
            .OrderByDescending(m => m.Inspections)
            .Take(2)
            .Aggregate(1L, (acc, curr) => acc * curr.Inspections);

        return monkeyBusiness;
    }
}