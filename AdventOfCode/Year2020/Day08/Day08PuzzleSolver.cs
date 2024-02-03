using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day08;

[Puzzle(2020, 8, "Handheld Halting")]
public class Day08PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var terminatedNormally = RunProgram(_instructions, out var accumulator);
        if (terminatedNormally)
        {
            throw new InvalidOperationException("Part one shouldn't have terminated normally");
        }

        return new PuzzleAnswer(accumulator, 1451);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        foreach (var fixedInstruction in _instructions.Where(instruction => instruction.InstructionType != InstructionType.Acc))
        {
            var fixedInstructions = _instructions
                .Select(instruction => instruction == fixedInstruction
                    ? instruction.GetFixed()
                    : instruction)
                .ToList();

            var terminatedNormally = RunProgram(fixedInstructions, out var accumulator);
            if (terminatedNormally)
            {
                answer = accumulator;
                break;
            }
        }

        return new PuzzleAnswer(answer, 1160);
    }

    private static bool RunProgram(List<Instruction> instructions, out int accumulator)
    {
        var visitedInstructionIndexes = new HashSet<int>();

        accumulator = 0;

        var instructionIndex = 0;
        while (instructionIndex < instructions.Count)
        {
            if (visitedInstructionIndexes.Contains(instructionIndex))
            {
                return false;
            }

            visitedInstructionIndexes.Add(instructionIndex);

            var instruction = instructions[instructionIndex];
            switch (instruction.InstructionType)
            {
                case InstructionType.Nop:
                    instructionIndex++;
                    break;
                case InstructionType.Acc:
                    accumulator += instruction.Argument;
                    instructionIndex++;
                    break;
                case InstructionType.Jmp:
                    instructionIndex += instruction.Argument;
                    break;
            }
        }

        return true;
    }
}