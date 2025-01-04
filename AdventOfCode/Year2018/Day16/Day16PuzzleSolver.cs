using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Year2018.Common;

namespace AdventOfCode.Year2018.Day16;

[Puzzle(2018, 16, "Chronal Classification")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private List<Sample> _samples = [];
    private List<long[]> _instructionValues = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _samples = chunks.SkipLast(1)
                         .Select(Sample.Parse)
                         .ToList();

        _instructionValues = chunks.TakeLast(1)
                                   .SelectMany(x => x.Select(y => y.SplitToNumbers<long>(' ')))
                                   .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _samples.Count(s => s.BehavesLikeThreeOpCodes());

        return new PuzzleAnswer(answer, 493);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var opCodes = Enum.GetValues<OpCode>();

        var opCodePossibilities = Enumerable.Range(0, opCodes.Length)
                                            .Select(x => opCodes.ToList())
                                            .ToList();

        foreach (var sample in _samples)
        {
            foreach (var opCode in opCodes)
            {
                if (!sample.BehavesLikeOpcode(opCode))
                {
                    opCodePossibilities[(int)sample.InstructionValues[0]].Remove(opCode);
                }
            }
        }

        var knownOpCodes = new HashSet<OpCode>();
        while (knownOpCodes.Count != opCodes.Length)
        {
            var opCodeValue = opCodePossibilities.FindIndex(0, x => x.Count == 1 && !knownOpCodes.Contains(x[0]));
            if (opCodeValue == -1)
            {
                throw new InvalidOperationException("Couldn't find opCode");
            }

            var opCode = opCodePossibilities[opCodeValue][0];

            for (var value = 0; value < opCodePossibilities.Count; value++)
            {
                if (value == opCodeValue)
                {
                    // Keep the known mapping
                    knownOpCodes.Add(opCode);
                    continue;
                }

                opCodePossibilities[value].Remove(opCode);
            }
        }

        // Execute instructions
        var device = new Device([0, 0, 0, 0]);

        foreach (var instructionValue in _instructionValues)
        {
            var opCode = opCodePossibilities[(int)instructionValue[0]][0];

            var instruction = Instruction.Parse(instructionValue);
            instruction.ChangeOpCode(opCode);

            device.RunInstruction(instruction);
        }

        var answer = device.Registers[0];

        return new PuzzleAnswer(answer, 445);
    }
}