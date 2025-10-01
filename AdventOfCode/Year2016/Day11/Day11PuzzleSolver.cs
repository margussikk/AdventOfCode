using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2016.Day11;

[Puzzle(2016, 11, "Radioisotope Thermoelectric Generators")]
public class Day11PuzzleSolver : IPuzzleSolver
{
    private State _initialState = null!;

    public void ParseInput(string[] inputLines)
    {
        _initialState = State.Parse(inputLines);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var initialState = _initialState.Clone();

        var answer = GetAnswer(initialState);

        return new PuzzleAnswer(answer, 31);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var initialState = _initialState.Clone();
        initialState.Items.Add(new Item { Element = "elerium", Type = ItemType.Generator, Floor = 1 });
        initialState.Items.Add(new Item { Element = "elerium", Type = ItemType.Microchip, Floor = 1 });
        initialState.Items.Add(new Item { Element = "dilithium", Type = ItemType.Generator, Floor = 1 });
        initialState.Items.Add(new Item { Element = "dilithium", Type = ItemType.Microchip, Floor = 1 });

        var answer = GetAnswer(initialState);

        return new PuzzleAnswer(answer, 55);
    }

    private static int GetAnswer(State initialState)
    {
        var answer = 0;

        initialState.SortItems();

        var visited = new HashSet<int>([initialState.GetStateHash()]);
        var queue = new Queue<State>();

        queue.Enqueue(initialState);
        while (queue.TryDequeue(out var state))
        {
            if (state.Items.All(i => i.Floor == 4))
            {
                answer = state.Steps;
                break;
            }

            var nextStates = state.BuildNextStates();
            foreach (var nextState in nextStates.Where(x => visited.Add(x.GetStateHash())))
            {
                queue.Enqueue(nextState);
            }
        }

        return answer;
    }
}