namespace AdventOfCode.Year2023.Day15;

internal class ReplaceLensOperation(string label, int focalLength) : Operation(label)
{
    public int FocalLength { get; private set; } = focalLength;

    public override string ToString()
    {
        return $"{Label}={FocalLength}";
    }
}
