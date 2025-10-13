namespace AdventOfCode.Year2015.Day07;

internal class NotGate : Gate
{
    public Port Input { get; }

    public NotGate()
    {
        Input = new Port(this);
    }

    public override void PerformLogic()
    {
        if (!Input.Signal.HasValue)
        {
            return;
        }

        OutputWire.CarrySignal((~Input.Signal.Value) & 0xFFFF);
    }
}
