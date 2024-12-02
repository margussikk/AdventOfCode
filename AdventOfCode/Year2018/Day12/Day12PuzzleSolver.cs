using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2018.Day12;

[Puzzle(2018, 12, "Subterranean Sustainability")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private List<bool> _initialState = [];

    private readonly bool[] _plantNotes = new bool[32];

    public void ParseInput(string[] inputLines)
    {
        _initialState = inputLines[0]["initial state: ".Length ..] .Select(x => x == '#')
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
        var answer = GetAnswer(20);

        return new PuzzleAnswer(answer, 2736);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("TODO", "TODO");
    }

    private int GetAnswer(int generations)
    {
        var zeroIndex = 0;

        var state = new List<bool>(_initialState);

        for (var generation = 0; generation < generations; generation++)
        {
            // Padding
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

            // Plant
            var newState = new List<bool>(state.Count)
            {
                false,
                false
            };

            for (var potIndex = 2; potIndex < state.Count - 2; potIndex++)
            {
                var pattern = GetPattern(state, potIndex);
                newState.Add(_plantNotes[pattern]);
            }
            newState.Add(false);
            newState.Add(false);

            state = newState;
        }

        return state.Select((x, i) => x ? i - zeroIndex : 0)
                    .Sum();
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