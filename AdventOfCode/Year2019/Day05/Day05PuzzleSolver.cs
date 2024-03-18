using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day05;

[Puzzle(2019, 5, "Sunny with a Chance of Asteroids")]
public class Day05PuzzleSolver : IPuzzleSolver
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

        return new PuzzleAnswer(answer, 9025675);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computer = new IntCodeComputer(_program);

        var result = computer.Run(5);

        var answer = result.Outputs[^1];

        return new PuzzleAnswer(answer, 11981754);
    }
}