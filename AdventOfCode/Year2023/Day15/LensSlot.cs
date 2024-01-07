namespace AdventOfCode.Year2023.Day15;

internal class LensSlot(string label, int focalLength)
{
    public string Label { get; } = label;

    public int FocalLength { get; set; } = focalLength;
}
