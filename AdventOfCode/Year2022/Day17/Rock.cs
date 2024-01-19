using System.Text;

namespace AdventOfCode.Year2022.Day17;

internal class Rock
{
    public int Row { get; set; }

    private int _column = 0;
    public int Column
    {
        get
        {
            return _column;
        }
        set
        {
            if (_column != value)
            {
                _column = value;
                BitMasks = InitialBitMasks.Select(x => x >> _column).ToArray();
            }
        }
    }

    protected int[] InitialBitMasks { get; private set; } = [];

    public int[] BitMasks { get; private set; } = [];

    public int Width { get; private set; }

    public static Rock Parse(params string[] lines)
    {
        var rock = new Rock
        {
            InitialBitMasks = new int[lines.Length],
            BitMasks = new int[lines.Length],
            Width = lines[0].Length,
        };

        // Bit masks
        for (var row = 0; row < lines.Length; row++)
        {
            var bitMask = 0;

            for (var column = 0; column < lines[row].Length; column++)
            {
                if (lines[row][column] == '#')
                {
                    bitMask |= 1 << 6 - column;
                }
            }

            // Fill rows in reverse order
            rock.InitialBitMasks[lines.Length - 1 - row] = bitMask;
        }

        rock.BitMasks = [.. rock.InitialBitMasks]; // Clone

        return rock;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        for (var row = BitMasks.Length - 1; row >= 0; row--)
        {
            var mask = 0b1_000_000;

            for (var i = 0; i < 7; i++)
            {
                if ((BitMasks[row] & mask) != 0)
                {
                    stringBuilder.Append('#');
                }
                else
                {
                    stringBuilder.Append('.');
                }
                mask >>= 1;
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }
}
