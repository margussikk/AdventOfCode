using System.Text;

namespace AdventOfCode.Year2019.IntCode;

internal class IntCodeComputer
{
    private readonly Dictionary<IntCodeOpCode, InstructionDetails> _instructionDetails = new()
    {
        [IntCodeOpCode.Add] = new(3, true),
        [IntCodeOpCode.Multiply] = new(3, true),
        [IntCodeOpCode.Input] = new(1, true),
        [IntCodeOpCode.Output] = new(1, false),
        [IntCodeOpCode.JumpIfTrue] = new(2, false),
        [IntCodeOpCode.JumpIfFalse] = new(2, false),
        [IntCodeOpCode.LessThan] = new(3, true),
        [IntCodeOpCode.Equals] = new(3, true),
        [IntCodeOpCode.AdjustRelativeBase] = new(1, false),
        [IntCodeOpCode.Halt] = new(0, false),
    };

    private long[] _memory = [];

    private long _instructionPointer = 0L;

    private long _relativeBase = 0L;

    public Queue<long> Inputs { get; } = new();

    public Queue<long> Outputs { get; } = new();

    public void Load(IntCodeProgram program)
    {
        _memory = new long[10_000];

        for (var i = 0; i < program.Code.Count; i++)
        {
            _memory[i] = program.Code[i];
        }
    }

    public void WriteMemory(long address, long value)
    {
        _memory[(int)address] = value;
    }

    public long ReadMemory(long address)
    {
        return _memory[(int)address];
    }

    public IntCodeExitCode Run()
    {
        while(_instructionPointer < _memory.Length)
        {
            var instruction = PrepareInstruction(_instructionPointer);

            switch (instruction.OpCode)
            {
                case IntCodeOpCode.Add: // 1
                    _memory[(int)instruction.Arguments[2]] = instruction.Arguments[0] + instruction.Arguments[1];

                    _instructionPointer += 4;
                    break;
                case IntCodeOpCode.Multiply: // 2
                    _memory[(int)instruction.Arguments[2]] = instruction.Arguments[0] * instruction.Arguments[1];

                    _instructionPointer += 4;
                    break;
                case IntCodeOpCode.Input: // 3
                    if (Inputs.TryDequeue(out var inputValue))
                    {
                        _memory[(int)instruction.Arguments[0]] = inputValue;
                        _instructionPointer += 2;

                        break;
                    }
                    else
                    {
                        return IntCodeExitCode.WaitingForInput;
                    }
                case IntCodeOpCode.Output: // 4
                    Outputs.Enqueue(instruction.Arguments[0]);

                    _instructionPointer += 2;
                    break;
                case IntCodeOpCode.JumpIfTrue: // 5
                    _instructionPointer = instruction.Arguments[0] != 0
                        ? instruction.Arguments[1]
                        : _instructionPointer + 3;

                    break;
                case IntCodeOpCode.JumpIfFalse: // 6
                    _instructionPointer = instruction.Arguments[0] == 0
                        ? instruction.Arguments[1]
                        : _instructionPointer + 3;

                    break;
                case IntCodeOpCode.LessThan: // 7
                    _memory[(int)instruction.Arguments[2]] = instruction.Arguments[0] < instruction.Arguments[1] ? 1 : 0;

                    _instructionPointer += 4;
                    break;
                case IntCodeOpCode.Equals: // 8
                    _memory[(int)instruction.Arguments[2]] = instruction.Arguments[0] == instruction.Arguments[1] ? 1 : 0;

                    _instructionPointer += 4;
                    break;
                case IntCodeOpCode.AdjustRelativeBase: // 9
                    _relativeBase += instruction.Arguments[0];

                    _instructionPointer += 2;
                    break;

                case IntCodeOpCode.Halt: // 99
                    return IntCodeExitCode.Halted;
                default:
                    throw new InvalidOperationException($"Invalid opCode {instruction.OpCode}");
            }
        }

        return IntCodeExitCode.Completed;
    }

    public void AddAsciiInput(string text)
    {
        foreach (var character in text)
        {
            Inputs.Enqueue(Convert.ToInt64(character));
        }
    }

    public string GetAsciiOutput()
    {
        var stringBuilder = new StringBuilder();
        while (Outputs.TryDequeue(out var output))
        {
            stringBuilder.Append(Convert.ToChar(output));
        }

        return stringBuilder.ToString();
    }

    private IntCodeInstruction PrepareInstruction(long pointer)
    {
        var instruction = new IntCodeInstruction
        {
            OpCode = (IntCodeOpCode)(_memory[(int)pointer] % 100)
        };

        // Parameters
        var instructionDetails = _instructionDetails[instruction.OpCode];
        instruction.Arguments = new long[instructionDetails.ParameterCount];

        var parameterModes = _memory[(int)pointer] / 100;
        for (var index = 0; index < instruction.Arguments.Length; index++)
        {
            var parameterMode = (IntCodeParameterMode)(parameterModes % 10);

            // Last parameter of an instruction that writes to memory always refers to memory position, either directly or relatively
            if (instructionDetails.WritesToMemory && index == instruction.Arguments.Length - 1)
            {
                var position = _memory[(int)pointer + index + 1];

                instruction.Arguments[index] = parameterMode switch
                {
                    IntCodeParameterMode.Position => position,
                    IntCodeParameterMode.Relative => _relativeBase + position,
                    _ => throw new InvalidOperationException($"Invalid parameter mode {parameterMode}")
                };
            }
            else
            {
                instruction.Arguments[index] = parameterMode switch
                {
                    IntCodeParameterMode.Position => _memory[(int)_memory[(int)pointer + index + 1]],
                    IntCodeParameterMode.Immediate => _memory[(int)pointer + index + 1],
                    IntCodeParameterMode.Relative => _memory[(int)_relativeBase + (int)_memory[(int)pointer + index + 1]],
                    _ => throw new InvalidOperationException($"Invalid parameter mode {parameterMode}")
                };
            }

            parameterModes /= 10;
        }

        return instruction;
    }

    private sealed record InstructionDetails(int ParameterCount, bool WritesToMemory);
}
