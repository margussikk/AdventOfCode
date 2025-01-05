using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Year2018.Common;
using Spectre.Console;

namespace AdventOfCode.Year2018.Day21;

[Puzzle(2018, 21, "")]
public class Day21PuzzleSolver : IPuzzleSolver
{
    private List<Instruction> _instructions = [];

    public void ParseInput(string[] inputLines)
    {
        if (!inputLines[0].StartsWith("#ip"))
        {
            throw new InvalidOperationException("First line in input wasn't #ip");
        }

        _instructions = inputLines.Skip(1)
                                  .Select(Instruction.Parse)
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GenerateRegister0Values().First();

        return new PuzzleAnswer(answer, 16311888L);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GenerateRegister0Values().Last();

        return new PuzzleAnswer(answer, 1413889);
    }

    // This is reverse engineered algorithm of the program
    private IEnumerable<long> GenerateRegister0Values()
    {
        var generatedRegister0Values = new HashSet<long>();

        var eqRRInstruction = _instructions.First(i => i.OpCode == OpCode.EqRR);
        var specialRegister = eqRRInstruction.A != 0 ? (int)eqRRInstruction.A : (int)eqRRInstruction.B;
        var seedValue = _instructions.First(i => i.OpCode == OpCode.SetI && i.A > 123 && i.C == specialRegister).A;

        var register2 = 0L;
        var register3 = 0L;

        while (true)
        {
            register2 = register3 | 65536L;      // bori 3 65536 2
            register3 = seedValue;               // seti 10736359 9 3
            while (true)
            {
                var register1 = register2 & 255; // bani 2 255 1
                register3 += register1;          // addr 3 1 3
                register3 &= 16777215;           // bani 3 16777215 3
                register3 *= 65899;              // muli 3 65899 3
                register3 &= 16777215;           // bani 3 16777215 3

                if (256 > register2)             // gtir 256 2 1
                {
                    break;
                }

                // The following instructions increase register 1 and register 5 until
                // they find register2 / 256
                // seti 0 3 1, addi 1 1 5, muli 5 256 5, gtrr 5 2 5, addr 5 4 4,
                // addi 4 1 4, seti 25 8 4, addi 1 1 1, seti 17 6 4, setr 1 5 2
                register2 /= 256;
            }

            if (generatedRegister0Values.Add(register3))
            {
                yield return register3;
            }
            else
            {
                break;
            }
        }
    }
}