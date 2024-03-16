namespace AdventOfCode.Year2022.Day21;

internal abstract class Monkey(string name)
{
    public string Name { get; } = name;

    public OperationMonkey? Parent { get; private set; }

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

        return new List<Monkey>(stack);
    }

    public abstract long YellNumber();
}
