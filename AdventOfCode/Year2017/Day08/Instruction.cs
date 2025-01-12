namespace AdventOfCode.Year2017.Day08;
internal class Instruction
{
    public string OperationRegister { get; private set; } = string.Empty;
    public Operation Operation { get; private set; }
    public int OperationValue { get; private set; }

    public string ConditionRegister { get; private set; } = string.Empty;
    public Condition Condition { get; private set; }
    public int ConditionValue { get; private set; }

    public static Instruction Parse(string input)
    {
        var splits = input.Split(' ');

        var instruction = new Instruction
        {
            OperationRegister = splits[0],
            Operation = splits[1] switch
            {
                "inc" => Operation.Inc,
                "dec" => Operation.Dec,
                _ => throw new InvalidOperationException($"Invalid instruction operation: {splits[1]}")
            },
            OperationValue = int.Parse(splits[2]),
            ConditionRegister = splits[4],
            Condition = splits[5] switch
            {
                ">" => Condition.Greater,
                ">=" => Condition.GreaterEqual,
                "<" => Condition.Less,
                "<=" => Condition.LessEqual,
                "==" => Condition.Equal,
                "!=" => Condition.NotEqual,
                _ => throw new InvalidOperationException($"Invalid instruction condition: {splits[5]}")
            },
            ConditionValue = int.Parse(splits[6]),
        };

        return instruction;
    }
}
