namespace AdventOfCode.Year2022.Day21;

internal class OperationMonkey : Monkey
{
    public Operation Operation { get; }

    public Monkey? LeftMonkey { get; private set; }

    public Monkey? RightMonkey { get; private set; }

    public OperationMonkey(string name, Operation operation) : base(name)
    {
        Operation = operation;
    }

    public void SetLeftMonkey(Monkey monkey)
    {
        LeftMonkey = monkey;
    }

    public void SetRightMonkey(Monkey monkey)
    {
        RightMonkey = monkey;
    }

    public override long YellNumber()
    {
        if (LeftMonkey == null || RightMonkey == null)
        {
            throw new InvalidOperationException();
        }

        var leftValue = LeftMonkey.YellNumber();
        var rightValue = RightMonkey.YellNumber();

        return Operation switch
        {
            Operation.Add => leftValue + rightValue,
            Operation.Subtract => leftValue - rightValue,
            Operation.Multiply => leftValue * rightValue,
            Operation.Divide => leftValue / rightValue,
            _ => throw new InvalidOperationException()
        };
    }
}
