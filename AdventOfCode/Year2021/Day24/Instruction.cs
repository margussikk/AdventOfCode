namespace AdventOfCode.Year2021.Day24;

internal class Instruction
{
    public InstructionCode Code { get; private set; }

    public IInstructionParameter? ParameterA { get; private set; }

    public IInstructionParameter? ParameterB { get; private set; }

    public static Instruction Parse(string line)
    {
        var instruction = new Instruction();

        var splits = line.Split(" ");

        instruction.Code = splits[0] switch
        {
            "inp" => InstructionCode.Inp,
            "add" => InstructionCode.Add,
            "mul" => InstructionCode.Mul,
            "div" => InstructionCode.Div,
            "mod" => InstructionCode.Mod,
            "eql" => InstructionCode.Eql,
            _ => throw new InvalidOperationException()
        };

        instruction.ParameterA = new VariableParameter(splits[1][0] - 'w');

        if (splits.Length == 2) // Only Inp
        {
            instruction.ParameterB = new NumberParameter(0);
        }
        else if (IsVariable(splits[2]))
        {
            instruction.ParameterB = new VariableParameter(splits[2][0] - 'w');
        }
        else
        {
            instruction.ParameterB = new NumberParameter(int.Parse(splits[2]));
        }

        return instruction;


        static bool IsVariable(string word)
        {
            return word[0] >= 'w' && word[0] <= 'z';
        }
    }

    public override string ToString()
    {
        if (ParameterA is not VariableParameter variableParameterA)
        {
            return string.Empty;
        }

        switch (Code)
        {
            case InstructionCode.Inp:
                return $"inp {Convert.ToChar('w' + variableParameterA.Variable)} = Input";
            case InstructionCode.Add when ParameterB is VariableParameter variableParameterB:
                return $"add {Convert.ToChar('w' + variableParameterA.Variable)} += {Convert.ToChar('w' + variableParameterB.Variable)}";
            case InstructionCode.Add when ParameterB is NumberParameter numberParameterB:
                return $"add {Convert.ToChar('w' + variableParameterA.Variable)} += {numberParameterB.Number}";
            case InstructionCode.Mul when ParameterB != null:
                switch (ParameterB)
                {
                    case VariableParameter variableParameterB:
                        return $"mul {Convert.ToChar('w' + variableParameterA.Variable)} *= {Convert.ToChar('w' + variableParameterB.Variable)}";
                    case NumberParameter numberParameterB:
                        return $"mul {Convert.ToChar('w' + variableParameterA.Variable)} *= {numberParameterB.Number}";
                }

                break;
            case InstructionCode.Div when ParameterB != null:
            {
                switch (ParameterB)
                {
                    case VariableParameter variableParameterB:
                        return $"div {Convert.ToChar('w' + variableParameterA.Variable)} /= {Convert.ToChar('w' + variableParameterB.Variable)}";
                    case NumberParameter numberParameterB:
                        return $"div {Convert.ToChar('w' + variableParameterA.Variable)} /= {numberParameterB.Number}";
                }

                break;
            }
            case InstructionCode.Mod when ParameterB != null:
            {
                switch (ParameterB)
                {
                    case VariableParameter variableParameterB:
                        return $"mod {Convert.ToChar('w' + variableParameterA.Variable)} %= {Convert.ToChar('w' + variableParameterB.Variable)}";
                    case NumberParameter numberParameterB:
                        return $"mod {Convert.ToChar('w' + variableParameterA.Variable)} %= {numberParameterB.Number}";
                }

                break;
            }
            case InstructionCode.Eql when ParameterB != null:
            {
                switch (ParameterB)
                {
                    case VariableParameter variableParameterB:
                        return $"eql {Convert.ToChar('w' + variableParameterA.Variable)} c= {Convert.ToChar('w' + variableParameterB.Variable)}";
                    case NumberParameter numberParameterB:
                        return $"eql {Convert.ToChar('w' + variableParameterA.Variable)} c= {numberParameterB.Number}";
                }

                break;
            }
        }

        return string.Empty;
    }
}
