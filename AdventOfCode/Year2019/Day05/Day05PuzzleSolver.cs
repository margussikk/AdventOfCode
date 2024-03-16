using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day05;

[Puzzle(2019, 5, "Sunny with a Chance of Asteroids")]
public class Day05PuzzleSolver : IPuzzleSolver
{
    private IntCodeProgram _program = new();

    public void ParseInput(string[] inputLines)
    {
        _program = IntCodeProgram.Parse(inputLines[0]);
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var computer = new IntCodeComputer();

        computer.Load(_program);

        computer.Inputs.Enqueue(1);

        computer.Run();

        var answer = computer.Outputs.Last();

        return new PuzzleAnswer(answer, 9025675);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computer = new IntCodeComputer();

        computer.Load(_program);

        computer.Inputs.Enqueue(5);

        computer.Run();

        var answer = computer.Outputs.Last();

        return new PuzzleAnswer(answer, 11981754);
    }
}