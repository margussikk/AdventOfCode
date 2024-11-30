namespace AdventOfCode.Year2023.Day15;

internal class Box
{
    public int Number { get; }

    private readonly List<LensSlot> _slots = [];

    public Box(int number)
    {
        Number = number;
    }

    public void RemoveLens(string label)
    {
        var slot = _slots.Find(x => x.Label == label);
        if (slot != null)
        {
            _slots.Remove(slot);
        }
    }

    public void ReplaceLens(string label, int focalLength)
    {
        var slot = _slots.Find(x => x.Label == label);
        if (slot != null)
        {
            slot.FocalLength = focalLength;
        }
        else
        {
            slot = new LensSlot(label, focalLength);
            _slots.Add(slot);
        }
    }

    public int CalculateFocalLength()
    {
        return _slots
            .Select((slot, index) => Number * (index + 1) * slot.FocalLength)
            .Sum();
    }
}
