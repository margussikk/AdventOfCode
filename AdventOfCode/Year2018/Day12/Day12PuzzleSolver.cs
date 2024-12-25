using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2018.Day12;

[Puzzle(2018, 12, "Subterranean Sustainability")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private List<bool> _initialState = [];

    private readonly bool[] _plantNotes = new bool[32];

    public void ParseInput(string[] inputLines)
    {
        _initialState = inputLines[0]["initial state: ".Length..].Select(x => x == '#')
                                     .ToList();

        foreach (string line in inputLines.Skip(2))
        {
            var splits = line.Split(" => ");

            var pattern = splits[0].Aggregate(0, (acc, curr) => acc * 2 + (curr == '#' ? 1 : 0));
            _plantNotes[pattern] = splits[1] == "#";
        }
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var zeroIndex = 0;

        var state = new List<bool>(_initialState);
        zeroIndex = AddPadding(state, zeroIndex);

        for (var generation = 1; generation <= 20; generation++)
        {
            state = PlantNextState(state);
            zeroIndex = AddPadding(state, zeroIndex);
        }

        var answer = SumOfPots(state, zeroIndex);

        return new PuzzleAnswer(answer, 2736);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var previousZeroIndex = 0;

        var previousState = new List<bool>(_initialState);
        previousZeroIndex = AddPadding(previousState, previousZeroIndex);

        for (var generation = 1; generation <= int.MaxValue; generation++)
        {
            var nextState = PlantNextState(previousState);
            var nextZeroIndex = AddPadding(nextState, previousZeroIndex);

            var previousStateStart = previousState.IndexOf(true);
            var previousStateEnd = previousState.LastIndexOf(true);

            var nextStateStart = nextState.IndexOf(true);
            var nextStateEnd = nextState.LastIndexOf(true);

            var same = previousState.Skip(previousStateStart)
                                    .Take(previousStateEnd - previousStateStart + 1)
                                    .SequenceEqual(nextState.Skip(nextStateStart)
                                                            .Take(nextStateEnd - nextStateStart + 1));

            if (same)
            {
                var previousSumOfPots = SumOfPots(previousState, previousZeroIndex);
                var nextSumOfPots = SumOfPots(nextState, nextZeroIndex);

                var answer = (50_000_000_000L - generation) * (nextSumOfPots - previousSumOfPots) + nextSumOfPots;

                return new PuzzleAnswer(answer, 3150000000905L);
            }

            previousState = nextState;
            previousZeroIndex = nextZeroIndex;
        }

        return new PuzzleAnswer("ERROR", "ERROR");
    }

    private static int SumOfPots(List<bool> state, int zeroIndex)
    {
        return state.Select((x, i) => x ? i - zeroIndex : 0)
                    .Sum();
    }

    private static int AddPadding(List<bool> state, int zeroIndex)
    {
        var leftPadding = 5 - state.IndexOf(true);

        for (var i = 0; i < leftPadding; i++)
        {
            state.Insert(0, false);
            zeroIndex++;
        }

        var rightPadding = 5 - (state.Count - 1 - state.LastIndexOf(true));
        for (var i = 0; i < rightPadding; i++)
        {
            state.Add(false);
        }

        return zeroIndex;
    }

    private List<bool> PlantNextState(List<bool> state)
    {
        var nextState = new List<bool>(state.Count)
        {
            false,
            false
        };

        for (var potIndex = 2; potIndex < state.Count - 2; potIndex++)
        {
            var pattern = GetPattern(state, potIndex);
            nextState.Add(_plantNotes[pattern]);
        }
        nextState.Add(false);
        nextState.Add(false);

        return nextState;
    }


    private static int GetPattern(List<bool> state, int potIndex)
    {
        var value = 0;

        for (var index = potIndex - 2; index < potIndex + 3; index++)
        {
            value *= 2;
            if (state[index])
            {
                value++;
            }
        }

        return value;
    }

    private static void Print(List<bool> state)
    {
        var value = new string(state.Select(x => x ? '#' : '.').ToArray());
        Console.WriteLine(value);
    }
}