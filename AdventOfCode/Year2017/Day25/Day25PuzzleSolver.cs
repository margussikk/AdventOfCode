using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Numerics;
using System.Collections;

namespace AdventOfCode.Year2017.Day25;

[Puzzle(2017, 25, "The Halting Problem")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private char _beginState = ' ';
    private int _steps = 0;

    private Dictionary<char, State> _states = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _beginState = chunks[0][0]["Begin in state ".Length..][0];
        _steps = int.Parse(chunks[0][1].Split(' ')[5]);

        _states = chunks.Skip(1)
                        .Select(State.Parse)
                        .ToDictionary(s => s.Name);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var bitArray = new BitArray(16384);
        var cursor = bitArray.Length / 2;
        var state = _states[_beginState];

        for (var step = 0; step < _steps; step++)
        {
            var stateAction = bitArray[cursor]
                ? state.TrueAction
                : state.FalseAction;

            // Write the value
            bitArray[cursor] = stateAction.Write;

            // Move one slot
            cursor += stateAction.Movement;

            // Continue with state
            state = _states[stateAction.NextStateName];
        }

        var answer = bitArray.PopCount();

        return new PuzzleAnswer(answer, 3578);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}