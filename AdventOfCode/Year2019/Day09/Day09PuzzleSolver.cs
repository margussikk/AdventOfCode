using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2019.IntCode;

namespace AdventOfCode.Year2019.Day09;

[Puzzle(2019, 9, "Sensor Boost")]
public class Day09PuzzleSolver : IPuzzleSolver
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

        var answer = computer.Outputs.Dequeue();

        return new PuzzleAnswer(answer, 2465411646L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var computer = new IntCodeComputer();
        computer.Load(_program);
        computer.Inputs.Enqueue(2);

        computer.Run();

        var answer = computer.Outputs.Dequeue();

        return new PuzzleAnswer(answer, 69781);
    }
}