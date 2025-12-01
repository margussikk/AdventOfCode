namespace AdventOfCode.Year2018.Common;

internal class Device
{
    public long[] Registers { get; private set; }

    public Device(IEnumerable<long> registers)
    {
        Registers = registers.ToArray();
    }

    public void RunInstruction(Instruction instruction)
    {
        if (!instruction.IsValid(Registers.Length))
        {
            throw new InvalidOperationException("Invalid instruction");
        }

        Registers[instruction.C] = instruction.OpCode switch
        {
            OpCode.AddR => Registers[instruction.A] + Registers[instruction.B],
            OpCode.AddI => Registers[instruction.A] + instruction.B,
            OpCode.MulR => Registers[instruction.A] * Registers[instruction.B],
            OpCode.MulI => Registers[instruction.A] * instruction.B,
            OpCode.BanR => Registers[instruction.A] & Registers[instruction.B],
            OpCode.BanI => Registers[instruction.A] & instruction.B,
            OpCode.BorR => Registers[instruction.A] | Registers[instruction.B],
            OpCode.BorI => Registers[(int)instruction.A] | instruction.B,
            OpCode.SetR => Registers[instruction.A],
            OpCode.SetI => instruction.A,
            OpCode.GtIR => instruction.A > Registers[instruction.B] ? 1 : 0,
            OpCode.GtRI => Registers[instruction.A] > instruction.B ? 1 : 0,
            OpCode.GtRR => Registers[instruction.A] > Registers[instruction.B] ? 1 : 0,
            OpCode.EqIR => instruction.A == Registers[instruction.B] ? 1 : 0,
            OpCode.EqRI => Registers[instruction.A] == instruction.B ? 1 : 0,
            OpCode.EqRR => Registers[instruction.A] == Registers[instruction.B] ? 1 : 0,
            _ => throw new NotImplementedException(),
        };
    }

    public void RunProgram(List<Instruction> instructions, int instructionPointerBinding, int breakAtInstructionPointer = -1)
    {
        var instructionPointer = 0;

        while (instructionPointer < instructions.Count)
        {
            if (instructionPointer == breakAtInstructionPointer)
            {
                return;
            }

            // Runs instruction
            var instruction = instructions[instructionPointer];

            RunInstruction(instruction);

            // Sets the instruction pointer to the value of bound register, and then adds one to the instruction pointer.
            instructionPointer = (int)Registers[instructionPointerBinding];
            instructionPointer++;

            // Updates bound register to the current instruction pointer value
            Registers[instructionPointerBinding] = instructionPointer;
        }
    }
}
