using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2015.Day23;

[Puzzle(2015, 23, "Opening the Turing Lock")]
public class Day23PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = [.. inputLines.Select(Instruction.Parse)];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var registers = new int[] { 0, 0 };
        var instructionPointer = 0;

        while (instructionPointer >= 0 && instructionPointer < _instructions.Count)
        {
            _instructions[instructionPointer].Execute(registers, ref instructionPointer);
        }

        var answer = registers[1];

        return new PuzzleAnswer(answer, 184);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var registers = new int[] { 1, 0 };
        var instructionPointer = 0;

        while (instructionPointer >= 0 && instructionPointer < _instructions.Count)
        {
            _instructions[instructionPointer].Execute(registers, ref instructionPointer);
        }

        var answer = registers[1];

        return new PuzzleAnswer(answer, 231);
    }
}