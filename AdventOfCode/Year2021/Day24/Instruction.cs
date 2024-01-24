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
        if (ParameterA == null)
        {
            return string.Empty;
        }

        if (ParameterA is not VariableParameter variableParameterA)
        {
            return string.Empty;
        }

        if (Code == InstructionCode.Inp)
        {
            return $"inp {Convert.ToChar('w' + variableParameterA.Variable)} = Input";
        }
        else if (Code == InstructionCode.Add)
        {
            if (ParameterB is VariableParameter variableParameterB)
            {
                return $"add {Convert.ToChar('w' + variableParameterA.Variable)} += {Convert.ToChar('w' + variableParameterB.Variable)}";
            }
            else if (ParameterB is NumberParameter numberParameterB)
            {
                return $"add {Convert.ToChar('w' + variableParameterA.Variable)} += {numberParameterB.Number}";
            }
        }
        else if (Code == InstructionCode.Mul && ParameterB != null)
        {
            if (ParameterB is VariableParameter variableParameterB)
            {
                return $"mul {Convert.ToChar('w' + variableParameterA.Variable)} *= {Convert.ToChar('w' + variableParameterB.Variable)}";
            }
            else if (ParameterB is NumberParameter numberParameterB)
            {
                return $"mul {Convert.ToChar('w' + variableParameterA.Variable)} *= {numberParameterB.Number}";
            }
        }
        else if (Code == InstructionCode.Div && ParameterB != null)
        {
            if (ParameterB is VariableParameter variableParameterB)
            {
                return $"div {Convert.ToChar('w' + variableParameterA.Variable)} /= {Convert.ToChar('w' + variableParameterB.Variable)}";
            }
            else if (ParameterB is NumberParameter numberParameterB)
            {
                return $"div {Convert.ToChar('w' + variableParameterA.Variable)} /= {numberParameterB.Number}";
            }
        }
        else if (Code == InstructionCode.Mod && ParameterB != null)
        {
            if (ParameterB is VariableParameter variableParameterB)
            {
                return $"mod {Convert.ToChar('w' + variableParameterA.Variable)} %= {Convert.ToChar('w' + variableParameterB.Variable)}";
            }
            else if (ParameterB is NumberParameter numberParameterB)
            {
                return $"mod {Convert.ToChar('w' + variableParameterA.Variable)} %= {numberParameterB.Number}";
            }
        }
        else if (Code == InstructionCode.Eql && ParameterB != null)
        {
            if (ParameterB is VariableParameter variableParameterB)
            {
                return $"eql {Convert.ToChar('w' + variableParameterA.Variable)} c= {Convert.ToChar('w' + variableParameterB.Variable)}";
            }
            else if (ParameterB is NumberParameter numberParameterB)
            {
                return $"eql {Convert.ToChar('w' + variableParameterA.Variable)} c= {numberParameterB.Number}";
            }
        }

        return string.Empty;
    }
}
