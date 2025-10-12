using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Utilities.Extensions;
internal static class CharExtensions
{
    public static bool IsGridDirection(this char source)
    {
        return source is '<' or '^' or '>' or 'v';
    }

    public static GridDirection ParseArrowToGridDirection(this char source)
    {
        return source switch
        {
            '<' => GridDirection.Left,
            '^' => GridDirection.Up,
            '>' => GridDirection.Right,
            'v' => GridDirection.Down,
            _ => throw new InvalidOperationException($"Unexpected direction character: {source}")
        };
    }

    public static GridDirection ParseLetterToGridDirection(this char source)
    {
        return source switch
        {
            'L' => GridDirection.Left,
            'U' => GridDirection.Up,
            'R' => GridDirection.Right,
            'D' => GridDirection.Down,
            _ => throw new InvalidOperationException($"Unexpected direction character: {source}")
        };
    }

    public static int ParseToDigit(this char source)
    {
        return source - '0';
    }
}
