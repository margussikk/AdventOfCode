using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2020.Day14;

internal class MaskInstruction: Instruction
{
    // For part one
    public long OneBitMask { get; private set; }

    public long ZeroBitMask { get; private set; }

    // For part two
    public long MemoryBitMask { get; private set; }

    public long FloatingBitMask { get; private set; }

    public static new MaskInstruction Parse(string input)
    {
        var maskInstruction = new MaskInstruction();

        // For part one
        foreach (char character in input)
        {
            maskInstruction.OneBitMask <<= 1;
            maskInstruction.ZeroBitMask <<= 1;

            if (character == '1')
            {
                maskInstruction.OneBitMask |= 1;
                maskInstruction.ZeroBitMask |= 1;
            }
            else if (character == '0')
            {
                // Do nothing
            }
            else if (character == 'X')
            {
                maskInstruction.ZeroBitMask |= 1;
            }
        }

        // For part two
        foreach (char character in input)
        {
            maskInstruction.MemoryBitMask <<= 1;
            maskInstruction.FloatingBitMask <<= 1;

            if (character == '1')
            {
                maskInstruction.MemoryBitMask |= 1;
            }
            else if (character == '0')
            {
                // Do nothing
            }
            else if (character == 'X')
            {
                maskInstruction.FloatingBitMask |= 1;
            }
        }


        return maskInstruction;
    }
}
