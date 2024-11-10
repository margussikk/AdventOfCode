namespace AdventOfCode.Year2021.Day24;

internal class Alu
{
    public long[] Variables = new long[4];

    private int _inputNumberIndex;

    public void Execute(int[] inputNumbers, List<Instruction> instructions)
    {
        Variables = new long[4];
        _inputNumberIndex = 0;

        foreach (var instruction in instructions)
        {
            if (instruction.ParameterA == null)
            {
                throw new InvalidOperationException("ParameterA");
            }

            if (instruction.ParameterA is not VariableParameter variableParameterA)
            {
                throw new InvalidOperationException("ParameterA not Variable");
            }

            if (instruction.Code == InstructionCode.Inp)
            {
                Variables[variableParameterA.Variable] = inputNumbers[_inputNumberIndex];
                _inputNumberIndex++;
            }
            else
            {
                if (instruction.ParameterB == null)
                {
                    throw new InvalidOperationException("ParameterB");
                }

                switch (instruction.Code)
                {
                    case InstructionCode.Add when instruction.ParameterB is VariableParameter variableParameterB:
                        Variables[variableParameterA.Variable] += Variables[variableParameterB.Variable];
                        break;
                    case InstructionCode.Add when instruction.ParameterB is NumberParameter numberParameterB:
                        Variables[variableParameterA.Variable] += numberParameterB.Number;
                        break;
                    case InstructionCode.Add:
                        throw new InvalidOperationException($"{InstructionCode.Add} ParameterB not Variable nor Number");
                    case InstructionCode.Mul when instruction.ParameterB is VariableParameter variableParameterB:
                        Variables[variableParameterA.Variable] *= Variables[variableParameterB.Variable];
                        break;
                    case InstructionCode.Mul when instruction.ParameterB is NumberParameter numberParameterB:
                        Variables[variableParameterA.Variable] *= numberParameterB.Number;
                        break;
                    case InstructionCode.Mul:
                        throw new InvalidOperationException($"{InstructionCode.Mul} ParameterB not Variable nor Number");
                    case InstructionCode.Div when instruction.ParameterB is VariableParameter variableParameterB:
                        Variables[variableParameterA.Variable] /= Variables[variableParameterB.Variable];
                        break;
                    case InstructionCode.Div when instruction.ParameterB is NumberParameter numberParameterB:
                        Variables[variableParameterA.Variable] /= numberParameterB.Number;
                        break;
                    case InstructionCode.Div:
                        throw new InvalidOperationException($"{InstructionCode.Div} ParameterB not Variable nor Number");
                    case InstructionCode.Mod when instruction.ParameterB is VariableParameter variableParameterB:
                        Variables[variableParameterA.Variable] %= Variables[variableParameterB.Variable];
                        break;
                    case InstructionCode.Mod when instruction.ParameterB is NumberParameter numberParameterB:
                        Variables[variableParameterA.Variable] %= numberParameterB.Number;
                        break;
                    case InstructionCode.Mod:
                        throw new InvalidOperationException($"{InstructionCode.Mod} ParameterB not Variable nor Number");
                    case InstructionCode.Eql when instruction.ParameterB is VariableParameter variableParameterB:
                        Variables[variableParameterA.Variable] = Variables[variableParameterA.Variable] == Variables[variableParameterB.Variable] ? 1 : 0;
                        break;
                    case InstructionCode.Eql when instruction.ParameterB is NumberParameter numberParameterB:
                        Variables[variableParameterA.Variable] = Variables[variableParameterA.Variable] == numberParameterB.Number ? 1 : 0;
                        break;
                    case InstructionCode.Eql:
                        throw new InvalidOperationException($"{InstructionCode.Eql} ParameterB not Variable nor Number");
                }
            }

            Console.WriteLine($"{instruction} // {this}");
        }
    }

    public override string ToString()
    {
        return $"inp={_inputNumberIndex}, w={Variables[0]}, x={Variables[1]}, y={Variables[2]}, z={Variables[3]}";
    }
}
