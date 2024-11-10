namespace AdventOfCode.Year2022.Day21;

internal class NumberMonkey : Monkey
{
    public long Number { get; }

    public NumberMonkey(string name, long number) : base(name)
    {
        Number = number;
    }
    
    public override long YellNumber()
    {
        return Number;
    }
}
