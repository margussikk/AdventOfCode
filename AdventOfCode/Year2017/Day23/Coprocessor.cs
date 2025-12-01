namespace AdventOfCode.Year2017.Day23;

internal class Coprocessor
{
    private readonly List<Instruction> _instructions;
    private int _instructionPointer;

    public long[] Registers { get; } = new long['h' - 'a' + 1];

    public int MulInvokes { get; private set; }

    public Coprocessor(List<Instruction> instructions)
    {
        _instructions = instructions;
    }

    public void Run(int breakpoint)
    {
        while (_instructionPointer >= 0 && _instructionPointer < _instructions.Count)
        {
            if (_instructionPointer == breakpoint)
            {
                break;
            }

            var instruction = _instructions[_instructionPointer];

            var operandAValue = instruction.OperandA.IsRegister
                ? Registers[instruction.OperandA.Register]
                : instruction.OperandA.Value;

            var operandBValue = instruction.OperandB.IsRegister
                ? Registers[instruction.OperandB.Register]
                : instruction.OperandB.Value;

            switch (instruction.Code)
            {
                case InstructionCode.Set:
                    Registers[instruction.OperandA.Register] = operandBValue;
                    break;
                case InstructionCode.Sub:
                    Registers[instruction.OperandA.Register] -= operandBValue;
                    break;
                case InstructionCode.Mul:
                    Registers[instruction.OperandA.Register] *= operandBValue;
                    MulInvokes++;
                    break;
                case InstructionCode.Jnz:
                    if (operandAValue != 0)
                    {
                        _instructionPointer += (int)operandBValue;
                    }
                    else
                    {
                        _instructionPointer++;
                    }

                    break;
            }

            if (instruction.Code != InstructionCode.Jnz)
            {
                _instructionPointer++;
            }
        }
    }
}
