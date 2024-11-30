namespace AdventOfCode.Year2023.Day15;

internal class RemoveLensOperation : Operation
{
    public RemoveLensOperation(string label) : base(label) { }

    public override string ToString()
    {
        return $"{Label}-";
    }
}
