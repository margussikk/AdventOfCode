using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2017.Day12;
internal class Pipe
{
    public int ProgramId { get; private set; }
    public int[] ConnectedToProgramIds { get; private set; } = [];

    public static Pipe Parse(string input)
    {
        var splits = input.Split("<->", StringSplitOptions.TrimEntries);

        var programConnection = new Pipe
        {
            ProgramId = int.Parse(splits[0]),
            ConnectedToProgramIds = splits[1].SplitToNumbers<int>(',', ' ')
        };

        return programConnection;
    }
}
