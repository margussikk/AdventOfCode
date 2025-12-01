using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day24;

internal class Component
{
    public ulong Bitmask { get; private set; }

    public int Port1 { get; private set; }

    public int Port2 { get; private set; }

    public int Strength => Port1 + Port2;

    public static Component Parse(string input)
    {
        var numbers = input.SplitToNumbers<int>('/');

        var component = new Component()
        {
            Bitmask = GetNextBitmask(),
            Port1 = numbers[0],
            Port2 = numbers[1]
        };

        return component;
    }

    private static int _counter = 0;
    private static ulong GetNextBitmask()
    {
        var bitmask = 1UL << _counter;
        _counter++;

        return bitmask;
    }
}
