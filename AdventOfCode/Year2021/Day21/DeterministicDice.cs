namespace AdventOfCode.Year2021.Day21;

internal class DeterministicDice
{
    private int _value = 1;

    public int RollCount { get; private set; }

    public int Roll()
    {
        RollCount++;

        var current = _value;
        _value = (_value + 1) % 100;

        return current;
    }
}
