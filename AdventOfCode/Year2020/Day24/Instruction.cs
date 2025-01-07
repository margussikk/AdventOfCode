using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2020.Day24;

internal class Instruction
{
    public List<GridDirection> Directions { get; private init; } = [];

    public static Instruction Parse(string input)
    {
        var directions = new List<GridDirection>();

        var span = input.AsSpan();
        while (span.Length > 0)
        {
            switch (span[0])
            {
                case 'e':
                    directions.Add(GridDirection.Right);
                    span = span[1..];
                    break;
                case 'w':
                    directions.Add(GridDirection.Left);
                    span = span[1..];
                    break;
                default:
                    {
                        switch (span[..2])
                        {
                            case "se":
                                directions.Add(GridDirection.DownRight);
                                break;
                            case "sw":
                                directions.Add(GridDirection.DownLeft);
                                break;
                            case "ne":
                                directions.Add(GridDirection.UpRight);
                                break;
                            case "nw":
                                directions.Add(GridDirection.UpLeft);
                                break;
                            default:
                                throw new InvalidOperationException("Failed to parse direction");
                        }

                        span = span[2..];

                        break;
                    }
            }
        }

        return new Instruction
        {
            Directions = directions
        };
    }
}
