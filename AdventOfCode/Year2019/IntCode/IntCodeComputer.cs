using System.Text;

namespace AdventOfCode.Year2019.IntCode;

internal class IntCodeComputer
{
    private readonly int[] _parameterModeDivisors = [100, 1_000, 10_000];

    public long[] Memory { get; }

    private int _instructionPointer = 0;

    private int _relativeBase = 0;

    public IntCodeComputer(IReadOnlyList<long> program)
    {
        Memory = new long[10_000];

        for (var i = 0; i < program.Count; i++)
        {
            Memory[i] = program[i];
        }
    }

    public IntCodeResult Run()
    {
        return Run(Array.Empty<long>());
    }

    public IntCodeResult Run(string input)
    {
        return Run([input]);
    }

    public IntCodeResult Run(IEnumerable<string> inputs)
    {
        var stringBuilder = new StringBuilder();

        foreach(var input in inputs)
        {
            stringBuilder.Append($"{input}\n");
        }

        var longInputs = new List<long>();

        foreach (var character in stringBuilder.ToString())
        {
            longInputs.Add(Convert.ToInt64(character));
        }

        return Run(longInputs);
    }

    public IntCodeResult Run(long input)
    {
        return Run([input]);
    }

    public IntCodeResult Run(IReadOnlyList<long> inputs)
    {
        int inputIndex = 0;
        var outputs = new List<long>();

        while(true)
        {
            var instruction = PrepareInstruction(_instructionPointer);
            switch (instruction.OpCode)
            {
                case IntCodeOpCode.Add: // 1
                    Memory[instruction.Addresses[2]] = Memory[instruction.Addresses[0]] + Memory[instruction.Addresses[1]];

                    _instructionPointer += instruction.Length;
                    break;
                case IntCodeOpCode.Multiply: // 2
                    Memory[instruction.Addresses[2]] = Memory[instruction.Addresses[0]] * Memory[instruction.Addresses[1]];

                    _instructionPointer += instruction.Length;
                    break;
                case IntCodeOpCode.Input: // 3
                    if (inputIndex >= inputs.Count)
                    {
                        return new IntCodeResult(IntCodeExitCode.WaitingForInput, outputs);
                    }

                    Memory[instruction.Addresses[0]] = inputs[inputIndex++];

                    _instructionPointer += instruction.Length;
                    break;
                case IntCodeOpCode.Output: // 4
                    outputs.Add(Memory[instruction.Addresses[0]]);

                    _instructionPointer += instruction.Length;
                    break;
                case IntCodeOpCode.JumpIfTrue: // 5
                    _instructionPointer = Memory[instruction.Addresses[0]] != 0
                        ? (int)Memory[instruction.Addresses[1]]
                        : _instructionPointer + instruction.Length;
                    break;
                case IntCodeOpCode.JumpIfFalse: // 6
                    _instructionPointer = Memory[instruction.Addresses[0]] == 0
                        ? (int)Memory[instruction.Addresses[1]]
                        : _instructionPointer + instruction.Length;
                    break;
                case IntCodeOpCode.LessThan: // 7
                    Memory[instruction.Addresses[2]] = Memory[instruction.Addresses[0]] < Memory[instruction.Addresses[1]] ? 1 : 0;

                    _instructionPointer += instruction.Length;
                    break;
                case IntCodeOpCode.Equals: // 8
                    Memory[instruction.Addresses[2]] = Memory[instruction.Addresses[0]] == Memory[instruction.Addresses[1]] ? 1 : 0;

                    _instructionPointer += instruction.Length;
                    break;
                case IntCodeOpCode.AdjustRelativeBase: // 9
                    _relativeBase += (int)Memory[instruction.Addresses[0]];

                    _instructionPointer += instruction.Length;
                    break;
                case IntCodeOpCode.Halt: // 99
                    return new IntCodeResult(IntCodeExitCode.Halted, outputs);
                default:
                    throw new InvalidOperationException($"Invalid opCode {instruction.OpCode}");
            }
        }
    }

    public IntCodeInstruction PrepareInstruction(int pointer)
    {
        var opCode = (IntCodeOpCode)(Memory[pointer] % 100);

        var instruction = new IntCodeInstruction
        {
            OpCode = opCode,
            Length = opCode switch
            {
                IntCodeOpCode.Halt => 1,
                IntCodeOpCode.AdjustRelativeBase or IntCodeOpCode.Input or IntCodeOpCode.Output => 2,
                IntCodeOpCode.JumpIfTrue or IntCodeOpCode.JumpIfFalse => 3,
                IntCodeOpCode.Add or IntCodeOpCode.Multiply or IntCodeOpCode.LessThan or IntCodeOpCode.Equals => 4,
                _ => throw new InvalidOperationException($"Invalid OpCode {opCode}")
            }
        };

        for (var parameterIndex = 0; parameterIndex < instruction.Length - 1; parameterIndex++)
        {
            var position = pointer + 1 + parameterIndex;

            var parameterMode = (IntCodeParameterMode)(Memory[pointer] / _parameterModeDivisors[parameterIndex] % 10);
            var address = parameterMode switch
            {
                IntCodeParameterMode.Position => (int)Memory[position],
                IntCodeParameterMode.Immediate => position,
                IntCodeParameterMode.Relative => _relativeBase + (int)Memory[position],
                _ => throw new InvalidOperationException($"Invalid parameter mode {parameterMode}")
            };

            instruction.Addresses.Add(address);
        }

        return instruction;
    }
}
