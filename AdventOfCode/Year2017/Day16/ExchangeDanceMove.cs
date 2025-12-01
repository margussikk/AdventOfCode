namespace AdventOfCode.Year2017.Day16;

internal class ExchangeDanceMove : IDanceMove
{
    public int PositionA { get; private set; }
    public int PositionB { get; private set; }

    public void DoMove(List<char> programs)
    {
        (programs[PositionB], programs[PositionA]) = (programs[PositionA], programs[PositionB]);
    }

    public static ExchangeDanceMove Parse(string input)
    {
        if (input[0] != 'x')
        {
            throw new InvalidOperationException("Exchange dance move must start with x");
        }

        var splits = input[1..].Split('/');

        return new ExchangeDanceMove
        {
            PositionA = int.Parse(splits[0]),
            PositionB = int.Parse(splits[1]),
        };
    }
}
