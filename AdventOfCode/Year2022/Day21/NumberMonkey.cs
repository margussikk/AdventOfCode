namespace AdventOfCode.Year2022.Day21;

internal class NumberMonkey(string name, long number) : Monkey(name)
{
    public long Number { get; } = number;

    public override long YellNumber()
    {
        return Number;
    }
}
