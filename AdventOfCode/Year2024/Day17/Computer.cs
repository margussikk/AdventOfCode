namespace AdventOfCode.Year2024.Day17;

internal class Computer
{
    private long _registerA;
    private long _registerB;
    private long _registerC;

    private readonly IReadOnlyList<int> _program;

    public Computer(long registerA, long registerB, long registerC, IReadOnlyList<int> program)
    {
        _registerA = registerA;
        _registerB = registerB;
        _registerC = registerC;
        _program = program;
    }

    public List<int> Run()
    {
        var output = new List<int>();
        var instructionPointer = 0;

        while (instructionPointer < _program.Count)
        {
            var instruction = (Instruction)_program[instructionPointer++];
            var operand = _program[instructionPointer++];

            switch (instruction)
            {
                case Instruction.Adv: // 0
                    _registerA >>= (int)GetComboOperandValue(operand);
                    break;
                case Instruction.Bxl: // 1
                    _registerB ^= operand;
                    break;
                case Instruction.Bst: // 2
                    _registerB = GetComboOperandValue(operand) % 8L;
                    break;
                case Instruction.Jnz: // 3
                    if (_registerA != 0)
                    {
                        instructionPointer = operand;
                    }
                    break;
                case Instruction.Bxc: // 4
                    _registerB ^= _registerC;
                    break;
                case Instruction.Out: // 5
                    output.Add((int)(GetComboOperandValue(operand) % 8));
                    break;
                case Instruction.Bdv: // 6
                    _registerB = _registerA >> (int)GetComboOperandValue(operand);
                    break;
                case Instruction.Cdv: // 7
                    _registerC = _registerA >> (int)GetComboOperandValue(operand);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        return output;
    }

    private long GetComboOperandValue(int operand)
    {
        return operand switch
        {
            >= 0 and <= 3 => operand,
            4 => _registerA,
            5 => _registerB,
            6 => _registerC,
            _ => throw new NotImplementedException()
        };
    }
}
