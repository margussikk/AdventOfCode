namespace AdventOfCode.Year2015.Day07;

internal class BufferGate : Gate
{
    public Port Input { get; }

    public BufferGate()
    {
        Input = new Port(this);
    }

    public override void PerformLogic()
    {
        if (!Input.Signal.HasValue)
        {
            return;
        }

        OutputWire.CarrySignal(Input.Signal.Value & 0xFFFF);
    }
}
