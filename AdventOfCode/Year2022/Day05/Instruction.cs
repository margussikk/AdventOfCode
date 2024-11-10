namespace AdventOfCode.Year2022.Day05;

internal class Instruction
{
    public int Quantity { get; private init; }

    public int FromStack { get; private init; }

    public int ToStack { get; private init; }

    public static Instruction Parse(string input)
    {
        var splits = input.Split(' ');
        if (splits is not ["move", _, "from", _, "to", _]) throw new NotImplementedException();

        return new Instruction
        {
            Quantity = int.Parse(splits[1]),
            FromStack = int.Parse(splits[3]) - 1,
            ToStack = int.Parse(splits[5]) - 1
        };
    }
}
