using AdventOfCode.Framework.Puzzle;
using System.Collections.Concurrent;

namespace AdventOfCode.Year2022.Day19;

[Puzzle(2022, 19, "Not Enough Minerals")]
public class Day19PuzzleSolver : IPuzzleSolver
{
    private List<Blueprint> _blueprints = [];

    public void ParseInput(string[] inputLines)
    {
        _blueprints = inputLines.Select(Blueprint.Parse)
                                .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        // Do in parallel for speed up
        var qualityLevels = new ConcurrentBag<int>();
        Parallel.ForEach(_blueprints, blueprint =>
        {
            var state = FindStateWithMaximumGeodes(blueprint, 24);
            if (state != null)
            {
                qualityLevels.Add(blueprint.Id * state.Geode);
            }
        });

        var answer = qualityLevels.Sum();

        return new PuzzleAnswer(answer, 994);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        // Do in parallel for speed up
        var geodes = new ConcurrentBag<int>();
        Parallel.ForEach(_blueprints.Take(3), blueprint =>
        {
            var state = FindStateWithMaximumGeodes(blueprint, 32);
            if (state != null)
            {
                geodes.Add(state.Geode);
            }
        });

        var answer = geodes.Aggregate(1, (acc, curr) => acc * curr);

        return new PuzzleAnswer(answer, 15960);
    }


    private static State? FindStateWithMaximumGeodes(Blueprint blueprint, int minutes)
    {
        var visited = new HashSet<State>();
        var maxGeodes = 0;
        State? maxState = null;

        var stack = new Stack<State>();

        var startState = new State
        {
            MinutesLeft = minutes,

            Ore = 0,
            Clay = 0,
            Obsidian = 0,
            Geode = 0,

            OreRobots = 1,
            ClayRobots = 0,
            ObsidianRobots = 0,
            GeodeRobots = 0,
        };

        stack.Push(startState);

        while (stack.TryPop(out var state))
        {
            if (state.MinutesLeft == 0)
            {
                if (state.Geode > maxGeodes)
                {
                    maxGeodes = state.Geode;
                    maxState = state;
                }
            }
            else
            {
                // Use geometric sum: Check if building new geode robot every minute left we could catch up to the best max
                var currentlyGeodesPossible = state.Geode + state.GeodeRobots * state.MinutesLeft + (state.MinutesLeft * (state.MinutesLeft - 1)) / 2;
                if (currentlyGeodesPossible < maxGeodes)
                {
                    continue;
                }

                if (visited.Contains(state))
                {
                    continue;
                }

                visited.Add(state);

                var nextStates = state.GetNextStates(blueprint);
                foreach (var nextState in nextStates)
                {
                    stack.Push(nextState);
                }
            }
        }

        return maxState;
    }
}