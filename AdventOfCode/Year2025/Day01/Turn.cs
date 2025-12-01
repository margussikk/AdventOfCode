using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2025.Day01;

internal class Turn
{
    public GridDirection Direction { get; private set; }
    public int Clicks { get; private set; }

    public static Turn Parse(string turnString)
    {
        return new Turn
        {
            Direction = turnString[0].ParseLetterToGridDirection(),
            Clicks = int.Parse(turnString[1..])
        };
    }
}
