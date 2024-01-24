namespace AdventOfCode.Year2021.Day24;

internal class Alu
{
    public long[] Variables = new long[4];

    private int _inputNumberIndex = 0;

    public bool Execute(int[] inputNumbers, List<Instruction> instructions)
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

                if (instruction.Code == InstructionCode.Add)
                {
                    if (instruction.ParameterB is VariableParameter variableParameterB)
                    {
                        Variables[variableParameterA.Variable] += Variables[variableParameterB.Variable];
                    }
                    else if (instruction.ParameterB is NumberParameter numberParameterB)
                    {
                        Variables[variableParameterA.Variable] += numberParameterB.Number;
                    }
                    else
                    {
                        throw new InvalidOperationException($"{InstructionCode.Add} ParameterB not Variable nor Number");
                    }
                }
                else if (instruction.Code == InstructionCode.Mul)
                {
                    if (instruction.ParameterB is VariableParameter variableParameterB)
                    {
                        Variables[variableParameterA.Variable] *= Variables[variableParameterB.Variable];
                    }
                    else if (instruction.ParameterB is NumberParameter numberParameterB)
                    {
                        Variables[variableParameterA.Variable] *= numberParameterB.Number;
                    }
                    else
                    {
                        throw new InvalidOperationException($"{InstructionCode.Mul} ParameterB not Variable nor Number");
                    }
                }
                else if (instruction.Code == InstructionCode.Div)
                {
                    if (instruction.ParameterB is VariableParameter variableParameterB)
                    {
                        Variables[variableParameterA.Variable] /= Variables[variableParameterB.Variable];
                    }
                    else if (instruction.ParameterB is NumberParameter numberParameterB)
                    {
                        Variables[variableParameterA.Variable] /= numberParameterB.Number;
                    }
                    else
                    {
                        throw new InvalidOperationException($"{InstructionCode.Div} ParameterB not Variable nor Number");
                    }
                }
                else if (instruction.Code == InstructionCode.Mod)
                {
                    if (instruction.ParameterB is VariableParameter variableParameterB)
                    {
                        Variables[variableParameterA.Variable] %= Variables[variableParameterB.Variable];
                    }
                    else if (instruction.ParameterB is NumberParameter numberParameterB)
                    {
                        Variables[variableParameterA.Variable] %= numberParameterB.Number;
                    }
                    else
                    {
                        throw new InvalidOperationException($"{InstructionCode.Mod} ParameterB not Variable nor Number");
                    }
                }
                else if (instruction.Code == InstructionCode.Eql)
                {
                    if (instruction.ParameterB is VariableParameter variableParameterB)
                    {
                        Variables[variableParameterA.Variable] = Variables[variableParameterA.Variable] == Variables[variableParameterB.Variable] ? 1: 0;
                    }
                    else if (instruction.ParameterB is NumberParameter numberParameterB)
                    {
                        Variables[variableParameterA.Variable] = Variables[variableParameterA.Variable] == numberParameterB.Number ? 1 : 0;
                    }
                    else
                    {
                        throw new InvalidOperationException($"{InstructionCode.Eql} ParameterB not Variable nor Number");
                    }

                }
            }

            Console.WriteLine($"{instruction} // {this}");
        }

        return true;
    }

    public override string ToString()
    {
        return $"inp={_inputNumberIndex}, w={Variables[0]}, x={Variables[1]}, y={Variables[2]}, z={Variables[3]}";
    }
}
