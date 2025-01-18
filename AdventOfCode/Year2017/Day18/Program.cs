namespace AdventOfCode.Year2017.Day18;
internal class Program
{
    private readonly List<Instruction> _instructions = [];
    private int _instructionPointer;

    public int Id { get; }

    public long[] Registers { get; } = new long['z' - 'a' + 1];

    public Queue<long> Inbox { get; } = [];

    public List<long> Outbox { get; } = [];

    public int SendCount { get; private set; }

    public Program(int id, List<Instruction> instructions)
    {
        Id = id;
        _instructions = instructions;
    }

    public void Run(bool partTwo)
    {
        while (_instructionPointer >= 0 && _instructionPointer < _instructions.Count)
        {
            var instruction = _instructions[_instructionPointer];

            var operandAValue = instruction.OperandA.IsRegister
                ? Registers[instruction.OperandA.Register]
                : instruction.OperandA.Value;

            var operandBValue = instruction.OperandB.IsRegister
                ? Registers[instruction.OperandB.Register]
                : instruction.OperandB.Value;

            switch (instruction.Code)
            {
                case InstructionCode.Snd:
                    Outbox.Add(operandAValue);
                    SendCount++;
                    break;
                case InstructionCode.Set:
                    Registers[instruction.OperandA.Register] = operandBValue;
                    break;
                case InstructionCode.Add:
                    Registers[instruction.OperandA.Register] += operandBValue;
                    break;
                case InstructionCode.Mul:
                    Registers[instruction.OperandA.Register] *= operandBValue;
                    break;
                case InstructionCode.Mod:
                    Registers[instruction.OperandA.Register] %= operandBValue;
                    break;
                case InstructionCode.Rcv:
                    if (!partTwo)
                    {
                        if (operandAValue != 0 && Inbox.Count == 0)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!Inbox.TryDequeue(out var result))
                        {
                            return;
                        }

                        Registers[instruction.OperandA.Register] = result;
                    }
                    break;
                case InstructionCode.Jgz:
                    if (operandAValue > 0)
                    {
                        _instructionPointer += (int)operandBValue;
                    }
                    else
                    {
                        _instructionPointer++;
                    }
                    
                    break;
            }

            if (instruction.Code != InstructionCode.Jgz)
            {
                _instructionPointer++;
            }
        }
    }
}
