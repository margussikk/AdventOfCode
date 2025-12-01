namespace AdventOfCode.Year2024.Day24;

internal class Wire
{
    public string Name { get; }

    public bool? Value { get; private set; }

    public List<Gate> OutputGates { get; } = [];

    public Wire(string name)
    {
        Name = name;
    }

    public void SendValue(bool value)
    {
        Value = value;

        foreach (var gate in OutputGates)
        {
            gate.SendValue(value);
        }
    }
}
