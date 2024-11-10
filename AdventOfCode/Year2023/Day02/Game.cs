namespace AdventOfCode.Year2023.Day02;

internal class Game
{
    public long Id { get; private set; }
    public IReadOnlyList<CubeSet> CubeSets { get; private init; } = [];

    public long Power
    {
        get
        {
            var maxCubeSet = CubeSets.Aggregate(new CubeSet(),
                (accumulator, current) =>
                {
                    accumulator.Red = Math.Max(accumulator.Red, current.Red);
                    accumulator.Green = Math.Max(accumulator.Green, current.Green);
                    accumulator.Blue = Math.Max(accumulator.Blue, current.Blue);

                    return accumulator;
                });

            return maxCubeSet.Red * maxCubeSet.Green * maxCubeSet.Blue;
        }
    }

    public static Game Parse(string input)
    {
        var splits = input.Split(':');

        var game = new Game
        {
            Id = long.Parse(splits[0].Replace("Game ", string.Empty)),
            CubeSets = splits[1]
                .Split(';')
                .Select(CubeSet.Parse)
                .ToList()
        };

        return game;
    }
}
