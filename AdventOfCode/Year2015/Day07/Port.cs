namespace AdventOfCode.Year2015.Day07;

internal class Port
{
    public int? Signal { get; set; }

    public Gate Gate { get; }

    public Port(Gate gate)
    {
        Gate = gate;
    }
}
