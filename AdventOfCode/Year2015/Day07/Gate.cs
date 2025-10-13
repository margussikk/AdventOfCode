namespace AdventOfCode.Year2015.Day07;

internal abstract class Gate
{
    public required Wire OutputWire { get; set; }

    public abstract void PerformLogic();
}
