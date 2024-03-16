using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2019.Day25;

internal class Room
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public List<string> Doors { get; private set; } = [];
    public List<string> Items { get; private set; } = [];

    public static Room Parse(string input)
    {
        var chunks = input.Split('\n')
                          .SelectToChunks();

        // Name and description
        var room = new Room
        {
            Name = chunks[0][0][3..^3],
            Description = chunks[0][1]
        };

        // Doors
        if (chunks[1][0] == "Doors here lead:")
        {
            room.Doors = chunks[1].Skip(1)
                                  .Select(x => x[2..])
                                  .ToList();
        }

        // Items
        if (chunks[2][0] == "Items here:")
        {
            room.Items = chunks[2].Skip(1)
                                  .Select(x => x[2..])
                                  .ToList();
        }

        return room;
    }
}
