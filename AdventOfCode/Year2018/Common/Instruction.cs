using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2018.Common;
internal class Instruction
{
    public OpCode OpCode { get; private set; }

    public int A { get; private set; }

    public int B { get; private set; }

    public int C { get; private set; }

    public void ChangeOpCode(OpCode opCode)
    {
        OpCode = opCode;
    }

    public bool IsValid(int registerCount)
    {
        var registerRange = new NumberRange<int>(0, registerCount - 1);

        if (OpCode is OpCode.AddR or OpCode.AddI
                   or OpCode.MulR or OpCode.MulI
                   or OpCode.BanR or OpCode.BanI
                   or OpCode.BorR or OpCode.BorI
                   or OpCode.SetR
                   or OpCode.GtRI or OpCode.GtRR
                   or OpCode.EqRI or OpCode.EqRR
                   && !registerRange.Contains(A))
        {
            return false;
        }

        if (OpCode is OpCode.AddR
                   or OpCode.MulR
                   or OpCode.BanR
                   or OpCode.BorR
                   or OpCode.GtIR or OpCode.GtRR
                   or OpCode.EqIR or OpCode.EqRR
                   && !registerRange.Contains(B))
        {
            return false;
        }

        return registerRange.Contains(C);
    }

    public static Instruction Parse(int[] instructionValues)
    {
        if (instructionValues.Length != 4)
        {
            throw new InvalidOperationException("Instruction too short");
        }

        return new Instruction
        {
            OpCode = (OpCode)instructionValues[0],
            A = instructionValues[1],
            B = instructionValues[2],
            C = instructionValues[3],
        };
    }
}
