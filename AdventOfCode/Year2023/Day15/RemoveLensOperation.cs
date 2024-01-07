namespace AdventOfCode.Year2023.Day15;

internal class RemoveLensOperation(string label) : Operation(label)
{
    public override string ToString()
    {
        return $"{Label}-";
    }
}
