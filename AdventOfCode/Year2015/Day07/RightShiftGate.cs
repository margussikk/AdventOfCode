namespace AdventOfCode.Year2015.Day07;

internal class RightShiftGate : Gate
{
    public Port Input1 { get; }
    public Port Input2 { get; }

    public RightShiftGate()
    {
        Input1 = new Port(this);
        Input2 = new Port(this);
    }

    public override void PerformLogic()
    {
        if (!Input1.Signal.HasValue || !Input2.Signal.HasValue)
        {
            return;
        }

        OutputWire.CarrySignal((Input1.Signal.Value >> Input2.Signal.Value) & 0xFFFF);
    }
}
