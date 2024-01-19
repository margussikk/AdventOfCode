namespace AdventOfCode.Year2022.Day21;

internal class OperationMonkey(string name, Operation operation) : Monkey(name)
{
    public Operation Operation { get; } = operation;

    public Monkey? LeftMonkey { get; private set; }

    public Monkey? RightMonkey { get; private set; }

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
