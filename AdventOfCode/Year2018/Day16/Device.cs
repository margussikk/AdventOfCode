namespace AdventOfCode.Year2018.Day16;
internal class Device
{
    public int[] Registers { get; private set; }

    public Device(IEnumerable<int> registers)
    {
        Registers = registers.ToArray();
    }

    public bool TryExecute(OpCode opCode, int[] instructionValues)
    {
        var instruction = Instruction.Parse(instructionValues);

        return opCode switch
        {
            OpCode.AddR => AddR(instruction),
            OpCode.AddI => AddI(instruction),
            OpCode.Mulr => MulR(instruction),
            OpCode.MulI => MulI(instruction),
            OpCode.BanR => BanR(instruction),
            OpCode.BanI => BanI(instruction),
            OpCode.BorR => BorR(instruction),
            OpCode.BorI => BorI(instruction),
            OpCode.SetR => SetR(instruction),
            OpCode.SetI => SetI(instruction),
            OpCode.GtIR => GtIR(instruction),
            OpCode.GtRI => GtRI(instruction),
            OpCode.GtRR => GtRR(instruction),
            OpCode.EqIR => EqIR(instruction),
            OpCode.EqRI => EqRI(instruction),
            OpCode.EqRR => EqRR(instruction),
            _ => throw new NotImplementedException()
        };
    }

    // addr (add register) stores into register C the result of adding register A and register B.
    private bool AddR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] + Registers[instruction.B];

        return true;
    }

    // addi (add immediate) stores into register C the result of adding register A and value B.
    private bool AddI(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] + instruction.B;

        return true;
    }

    // mulr (multiply register) stores into register C the result of multiplying register A and register B.
    private bool MulR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] * Registers[instruction.B];

        return true;
    }

    // muli (multiply immediate) stores into register C the result of multiplying register A and value B.
    private bool MulI(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] * instruction.B;

        return true;
    }

    // banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
    private bool BanR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] & Registers[instruction.B];

        return true;
    }

    // bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
    private bool BanI(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] & instruction.B;

        return true;
    }

    // borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
    private bool BorR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] | Registers[instruction.B];

        return true;
    }

    // bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
    private bool BorI(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] | instruction.B;

        return true;
    }

    // setr (set register) copies the contents of register A into register C. (Input B is ignored.)
    private bool SetR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A];

        return true;
    }

    // seti (set immediate) stores value A into register C. (Input B is ignored.)
    private bool SetI(Instruction instruction)
    {
        if (!IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = instruction.A;

        return true;
    }

    // gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
    private bool GtIR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = instruction.A > Registers[instruction.B] ? 1 : 0;

        return true;
    }

    // gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
    private bool GtRI(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] > instruction.B ? 1 : 0;

        return true;
    }

    // gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
    private bool GtRR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] > Registers[instruction.B] ? 1 : 0;

        return true;
    }

    // eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
    private bool EqIR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = instruction.A == Registers[instruction.B] ? 1 : 0;

        return true;
    }

    // eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
    private bool EqRI(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] == instruction.B ? 1 : 0;

        return true;
    }

    // eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
    private bool EqRR(Instruction instruction)
    {
        if (!IsValidRegister(instruction.A) || !IsValidRegister(instruction.B) || !IsValidRegister(instruction.C))
        {
            return false;
        }

        Registers[instruction.C] = Registers[instruction.A] == Registers[instruction.B] ? 1 : 0;

        return true;
    }

    private bool IsValidRegister(int register)
    {
        return register >= 0 && register < Registers.Length;
    }

    private sealed class Instruction
    {
        public int Opcode { get; private set; }

        public int A { get; private set; }

        public int B { get; private set; }

        public int C { get; private set; }

        public static Instruction Parse(int[] instructionValues)
        {
            if (instructionValues.Length != 4)
            {
                throw new InvalidOperationException("Instruction too short");
            }

            return new Instruction
            {
                Opcode = instructionValues[0],
                A = instructionValues[1],
                B = instructionValues[2],
                C = instructionValues[3],
            };
        }        
    }
}
