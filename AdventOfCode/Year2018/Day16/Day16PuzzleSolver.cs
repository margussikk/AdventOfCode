using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2018.Day16;

[Puzzle(2018, 16, "Chronal Classification")]
public class Day16PuzzleSolver : IPuzzleSolver
{
    private List<Sample> _samples = [];
    private List<int[]> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _samples = chunks.SkipLast(1)
                         .Select(Sample.Parse)
                         .ToList();

        _instructions = chunks.TakeLast(1)
                              .SelectMany(x => x.Select(y => y.Split(' ')
                                                              .Select(int.Parse)
                                                              .ToArray()))
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

        foreach(var sample in _samples)
        {
            foreach (var opCode in opCodes)
            {
                if (!sample.BehavesLikeOpcode(opCode))
                {
                    opCodePossibilities[sample.Instruction[0]].Remove(opCode);
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

        foreach (var instruction in _instructions)
        {
            var opCode = opCodePossibilities[instruction[0]][0];

            var success = device.TryExecute(opCode, instruction);
            if (!success)
            {
                throw new NotImplementedException();
            }
        }

        var answer = device.Registers[0];

        return new PuzzleAnswer(answer, 445);
    }
}