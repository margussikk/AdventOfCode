namespace AdventOfCode.Year2015.Day07;

internal class Wire
{
    public string Name { get; }

    public int? Signal { get; private set; }

    public List<Port> Ports { get; } = [];

    public Wire(string name)
    {
        Name = name;
    }

    public void CarrySignal(int? signal)
    {
        Signal = signal;

        foreach (var port in Ports)
        {
            port.Signal = Signal;
            port.Gate.PerformLogic();
        }
    }
}
