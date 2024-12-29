using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2018.Common;
internal class Instruction
{
    public OpCode OpCode { get; private set; }

    public long A { get; private set; }

    public long B { get; private set; }

    public long C { get; private set; }

    public void ChangeOpCode(OpCode opCode)
    {
        OpCode = opCode;
    }

    public bool IsValid(int registerCount)
    {
        var registerRange = new NumberRange<long>(0, registerCount - 1);

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

    public override string ToString()
    {
        var output = OpCode switch
        {
            OpCode.AddR => $"R{C} = R{A} + R{B}",
            OpCode.AddI => $"R{C} = R{A} + {B}",
            OpCode.MulR => $"R{C} = R{A} * R{B}",
            OpCode.MulI => $"R{C} = R{A} * {B}",
            OpCode.BanR => $"R{C} = R{A} & R{B}",
            OpCode.BanI => $"R{C} = R{A} & {B}",
            OpCode.BorR => $"R{C} = R{A} | R{B}",
            OpCode.BorI => $"R{C} = R{A} | {B}",
            OpCode.SetR => $"R{C} = R{A}",
            OpCode.SetI => $"R{C} = {A}",
            OpCode.GtIR => $"R{C} = {A} > R{B} ? 1 : 0",
            OpCode.GtRI => $"R{C} = R{A} > {B} ? 1 : 0",
            OpCode.GtRR => $"R{C} = R{A} > R{B} ? 1 : 0",
            OpCode.EqIR => $"R{C} = {A} == R{B} ? 1 : 0",
            OpCode.EqRI => $"R{C} = R{A} == {B} ? 1 : 0",
            OpCode.EqRR => $"R{C} = R{A} == R{B} ? 1 : 0",

            _ => throw new NotImplementedException()
        };

        return output;
    }

    public static Instruction Parse(long[] instructionValues)
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

    public static Instruction Parse(string input)
    {
        var splits = input.Split(' ');

        var opCode = splits[0] switch
        {
            "addr" => OpCode.AddR,
            "addi" => OpCode.AddI,
            "mulr" => OpCode.MulR,
            "muli" => OpCode.MulI,
            "banr" => OpCode.BanR,
            "bani" => OpCode.BanI,
            "borr" => OpCode.BorR,
            "bori" => OpCode.BorI,
            "setr" => OpCode.SetR,
            "seti" => OpCode.SetI,
            "gtir" => OpCode.GtIR,
            "gtri" => OpCode.GtRI,
            "gtrr" => OpCode.GtRR,
            "eqir" => OpCode.EqIR,
            "eqri" => OpCode.EqRI,
            "eqrr" => OpCode.EqRR,
            _ => throw new InvalidOperationException($"Invalid OpCode: {splits[0]}")
        };

        var a = int.Parse(splits[1]);
        var b = int.Parse(splits[2]);
        var c = int.Parse(splits[3]);

        return Parse([(int)opCode, a, b, c]);
    }
}
