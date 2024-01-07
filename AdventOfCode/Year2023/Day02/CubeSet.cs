namespace AdventOfCode.Year2023.Day02;

internal class CubeSet
{
    public long Red { get; set; }

    public long Green { get; set; }

    public long Blue { get; set; }

    public static CubeSet Parse(string input)
    {
        var cubeSet = input
            .Split(',')
            .Aggregate(new CubeSet(), (accumulator, current) =>
            {
                var splits = current.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var amount = long.Parse(splits[0]);

                switch (splits[1])
                {
                    case "red":
                        accumulator.Red += amount;
                        break;
                    case "green":
                        accumulator.Green += amount;
                        break;
                    case "blue":
                        accumulator.Blue += amount;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                return accumulator;
            });

        return cubeSet;
    }
}
