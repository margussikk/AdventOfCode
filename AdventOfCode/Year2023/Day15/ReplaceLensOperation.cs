namespace AdventOfCode.Year2023.Day15;

internal class ReplaceLensOperation : Operation
{
    public int FocalLength { get; }

    public ReplaceLensOperation(string label, int focalLength) : base(label)
    {
        FocalLength = focalLength;
    }
    
    public override string ToString()
    {
        return $"{Label}={FocalLength}";
    }
}
