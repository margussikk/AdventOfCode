using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2020.Day14;

[Puzzle(2020, 14, "Docking Data")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        _instructions = inputLines.Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var memory = new Dictionary<long, long>();

        var oneBitMask = 0L;
        var zeroBitMask = 0L;

        foreach(var instruction in _instructions)
        {
            switch (instruction)
            {
                case MaskInstruction maskInstruction:
                    oneBitMask = maskInstruction.OneBitMask;
                    zeroBitMask = maskInstruction.ZeroBitMask;
                    break;
                case MemInstruction memInstruction:
                {
                    var value = memInstruction.Value;
                    value |= oneBitMask;
                    value &= zeroBitMask;

                    memory[memInstruction.Address] = value;
                    break;
                }
                default:
                    throw new NotImplementedException();
            }
        }

        var answer = memory.Values.Sum();

        return new PuzzleAnswer(answer, 9879607673316L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var memory = new Dictionary<long, long>();

        var memoryBitMask = 0L;
        var floatingBitMask = 0L;

        foreach(var instruction in _instructions)
        {
            switch (instruction)
            {
                case MaskInstruction maskInstruction:
                    memoryBitMask = maskInstruction.MemoryBitMask;
                    floatingBitMask = maskInstruction.FloatingBitMask;
                    break;
                case MemInstruction memInstruction:
                {
                    var fixedAddress = memInstruction.Address | memoryBitMask;

                    List<long> addresses = [fixedAddress];

                    var bitMask = 1L << 36;
                    while (bitMask != 0L)
                    {
                        if ((floatingBitMask & bitMask) == bitMask)
                        {
                            var floatingAddresses = new List<long>();

                            foreach (var address in addresses)
                            {
                                floatingAddresses.Add(address | bitMask);
                                floatingAddresses.Add(address & ~bitMask);
                            }

                            addresses = floatingAddresses;
                        }

                        bitMask >>= 1;
                    }

                    foreach (var address in addresses)
                    {
                        memory[address] = memInstruction.Value;
                    }

                    break;
                }
                default:
                    throw new NotImplementedException();
            }
        }

        var answer = memory.Values.Sum();

        return new PuzzleAnswer(answer, 3435342392262L);
    }
}