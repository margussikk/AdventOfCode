namespace AdventOfCode.Year2020.Day14;

internal class MaskInstruction: Instruction
{
    // For part one
    public long OneBitMask { get; private set; }

    public long ZeroBitMask { get; private set; }

    // For part two
    public long MemoryBitMask { get; private set; }

    public long FloatingBitMask { get; private set; }

    public new static MaskInstruction Parse(string input)
    {
        var maskInstruction = new MaskInstruction();

        // For part one
        foreach (var character in input)
        {
            maskInstruction.OneBitMask <<= 1;
            maskInstruction.ZeroBitMask <<= 1;

            switch (character)
            {
                case '1':
                    maskInstruction.OneBitMask |= 1;
                    maskInstruction.ZeroBitMask |= 1;
                    break;
                case '0':
                    // Do nothing
                    break;
                case 'X':
                    maskInstruction.ZeroBitMask |= 1;
                    break;
            }
        }

        // For part two
        foreach (var character in input)
        {
            maskInstruction.MemoryBitMask <<= 1;
            maskInstruction.FloatingBitMask <<= 1;

            switch (character)
            {
                case '1':
                    maskInstruction.MemoryBitMask |= 1;
                    break;
                case '0':
                    // Do nothing
                    break;
                case 'X':
                    maskInstruction.FloatingBitMask |= 1;
                    break;
            }
        }

        return maskInstruction;
    }
}
