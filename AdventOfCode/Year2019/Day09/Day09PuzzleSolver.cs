using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day09;

[Puzzle(2019, 9, "Sensor Boost")]
public class Day09PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<long> _program = [];

    public void ParseInput(string[] inputLines)
    {
        _program = inputLines[0].SelectToLongs(',');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computer = new IntCodeComputer(_program);

        var result = computer.Run(1);

        var answer = result.Outputs[^1];

        return new PuzzleAnswer(answer, 2465411646L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computer = new IntCodeComputer(_program);

        var result = computer.Run(2);

        var answer = result.Outputs[^1];

        return new PuzzleAnswer(answer, 69781);
    }
}