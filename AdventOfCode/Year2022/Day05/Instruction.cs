namespace Day05;

internal class Instruction
{
    public int Quantity { get; private set; }

    public int FromStack { get; private set; }

    public int ToStack { get; private set; }

    public static Instruction Parse(string input)
    {
        var splits = input.Split(' ');
        if (splits.Length != 6 ||
            splits[0] != "move" ||
            splits[2] != "from" ||
            splits[4] != "to") throw new NotImplementedException();

        return new Instruction
        {
            Quantity = int.Parse(splits[1]),
            FromStack = int.Parse(splits[3]) - 1,
            ToStack = int.Parse(splits[5]) - 1,
        };
    }
}
