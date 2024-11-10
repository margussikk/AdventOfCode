namespace AdventOfCode.Year2023.Day15;

internal class LensSlot
{
    public string Label { get; }

    public int FocalLength { get; set; }

    public LensSlot(string label, int focalLength)
    {
        Label = label;
        FocalLength = focalLength;
    }
}
