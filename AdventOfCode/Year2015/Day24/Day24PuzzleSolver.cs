using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Collections;

namespace AdventOfCode.Year2015.Day24;

[Puzzle(2015, 24, "It Hangs in the Balance")]
public class Day24PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<int> _weights = [];

    public void ParseInput(string[] inputLines)
    {
        _weights = [.. inputLines.Select(int.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(3);

        return new PuzzleAnswer(answer, 10439961859L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(4);

        return new PuzzleAnswer(answer, 72050269L);
    }

    private long GetAnswer(int groupCount)
    {
        var goalWeight = _weights.Sum() / groupCount;

        var answerGroupSize = _weights.Count;
        var answer = long.MaxValue;

        var states = _weights
            .Select(weight => new State { Group = [weight], GroupWeight = weight })
            .ToList();

        var visited = states.Select(s => new ListKey<int>(s.Group)).ToHashSet();

        var queue = new Queue<State>(states);
        while (queue.TryDequeue(out var state))
        {
            foreach (var weight in _weights.Except(state.Group))
            {
                var newGroup = state.Group
                    .Append(weight)
                    .Order()
                    .ToList();

                var groupWeight = state.GroupWeight + weight;
                if (groupWeight == goalWeight)
                {
                    answerGroupSize = newGroup.Count;
                    var quantumEntanglement = newGroup.Aggregate(1L, (prod, item) => prod * item);
                    answer = long.Min(quantumEntanglement, answer);
                }
                else if (newGroup.Count >= answerGroupSize)
                {
                    return answer;
                }
                else if (groupWeight < goalWeight)
                {
                    var key = new ListKey<int>(newGroup);
                    if (visited.Add(key))
                    {
                        var newState = new State
                        {
                            Group = newGroup,
                            GroupWeight = groupWeight
                        };

                        queue.Enqueue(newState);
                    }
                }
            }
        }

        return 0L;
    }
}