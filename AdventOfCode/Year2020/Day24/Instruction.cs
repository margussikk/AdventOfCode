using AdventOfCode.Utilities.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2020.Day24;

internal class Instruction
{
    public List<GridDirection> Directions { get; private set; } = [];

    public static Instruction Parse(string input)
    {
        var directions = new List<GridDirection>();

        var span = input.AsSpan();
        while(span.Length > 0)
        {
            if (span[0] == 'e')
            {
                directions.Add(GridDirection.Right);
                span = span[1..];
            }
            else if (span[0] == 'w')
            {
                directions.Add(GridDirection.Left);
                span = span[1..];
            }
            else if (span[..2].SequenceEqual("se"))
            {
                directions.Add(GridDirection.DownRight);
                span = span[2..];
            }
            else if (span[..2].SequenceEqual("sw"))
            {
                directions.Add(GridDirection.DownLeft);
                span = span[2..];
            }
            else if (span[..2].SequenceEqual("ne"))
            {
                directions.Add(GridDirection.UpRight);
                span = span[2..];
            }
            else if (span[..2].SequenceEqual("nw"))
            {
                directions.Add(GridDirection.UpLeft);
                span = span[2..];
            }
            else
            {
                throw new InvalidOperationException("Failed to parse direction");
            }
        }

        return new Instruction
        {
            Directions = directions,
        };
    }
}
