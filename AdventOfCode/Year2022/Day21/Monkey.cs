namespace AdventOfCode.Year2022.Day21;

internal abstract class Monkey
{
    public string Name { get; }

    public OperationMonkey? Parent { get; private set; }

    protected Monkey(string name)
    {
        Name = name;
    }

    public void SetParent(OperationMonkey parent)
    {
        Parent = parent;
    }

    public List<Monkey> GetRouteFromRoot()
    {
        var stack = new Stack<Monkey>();

        var current = this;
        while (current != null)
        {
            stack.Push(current);
            current = current.Parent;
        }

        return [.. stack];
    }

    public abstract long YellNumber();
}
