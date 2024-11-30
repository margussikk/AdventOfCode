using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;
using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2020.Day20;

internal class Tile
{
    public int Id { get; private init; }

    public BitGrid Image { get; private init; } = new(0, 0);

    public uint[,] BorderBitmasks { get; private set; } = new uint[0, 0];

    public BitGrid GetOrientedImage(int orientation)
    {
        return orientation switch
        {
            0 => Image,
            1 => Image.RotateClockwise(),
            2 => Image.RotateClockwise().RotateClockwise(),
            3 => Image.RotateCounterClockwise(),
            4 => Image.FlipHorizontally(),
            5 => Image.FlipHorizontally().RotateClockwise(),
            6 => Image.FlipHorizontally().RotateClockwise().RotateClockwise(),
            7 => Image.FlipHorizontally().RotateCounterClockwise(),
            _ => throw new InvalidOperationException("Invalid orientation")
        };
    }

    public static Tile Parse(string[] lines)
    {
        var tile = new Tile
        {
            Id = int.Parse(lines[0]["Tile ".Length..^1]),
            Image = lines.Skip(1).ToArray().SelectToBitGrid(character => character == '#')
        };

        if (tile.Image.Height != tile.Image.Width)
        {
            throw new InvalidOperationException("Expected tile to be a square");
        }

        PrepareBitmasks(tile);

        return tile;
    }

    private static void PrepareBitmasks(Tile tile)
    {
        tile.BorderBitmasks = new uint[8, 4];

        // Orientation 0, input
        for (var column = 0; column <= tile.Image.LastColumnIndex; column++)
        {
            // Top
            tile.BorderBitmasks[0, TileBorder.Top] <<= 1;
            if (tile.Image[0, column])
            {
                tile.BorderBitmasks[0, TileBorder.Top]++;
            }

            // Bottom
            tile.BorderBitmasks[0, TileBorder.Bottom] <<= 1;
            if (tile.Image[tile.Image.LastRowIndex, column])
            {
                tile.BorderBitmasks[0, TileBorder.Bottom]++;
            }
        }

        for (var row = 0; row <= tile.Image.LastRowIndex; row++)
        {
            // Left
            tile.BorderBitmasks[0, TileBorder.Left] <<= 1;
            if (tile.Image[row, 0])
            {
                tile.BorderBitmasks[0, TileBorder.Left]++;
            }

            // Right
            tile.BorderBitmasks[0, TileBorder.Right] <<= 1;
            if (tile.Image[row, tile.Image.LastColumnIndex])
            {
                tile.BorderBitmasks[0, TileBorder.Right]++;
            }
        }

        // Orientations 1..3, previous orientation rotated clockwise
        for (var orientation = 1; orientation <= 3; orientation++)
        {
            tile.BorderBitmasks[orientation, TileBorder.Top] = BitFunctions.ReverseBits(tile.BorderBitmasks[orientation - 1, TileBorder.Left], tile.Image.Width);
            tile.BorderBitmasks[orientation, TileBorder.Right] = tile.BorderBitmasks[orientation - 1, TileBorder.Top];
            tile.BorderBitmasks[orientation, TileBorder.Bottom] = BitFunctions.ReverseBits(tile.BorderBitmasks[orientation - 1, TileBorder.Right], tile.Image.Width);
            tile.BorderBitmasks[orientation, TileBorder.Left] = tile.BorderBitmasks[orientation - 1, TileBorder.Bottom];
        }

        // Orientation 4, orientation 0 flipped horizontally
        tile.BorderBitmasks[4, TileBorder.Top] = BitFunctions.ReverseBits(tile.BorderBitmasks[0, TileBorder.Top], tile.Image.Width);
        tile.BorderBitmasks[4, TileBorder.Right] = tile.BorderBitmasks[0, TileBorder.Left];
        tile.BorderBitmasks[4, TileBorder.Bottom] = BitFunctions.ReverseBits(tile.BorderBitmasks[0, TileBorder.Bottom], tile.Image.Width);
        tile.BorderBitmasks[4, TileBorder.Left] = tile.BorderBitmasks[0, TileBorder.Right];

        // Orientations 5..7, previous orientation rotated clockwise
        for (var orientation = 5; orientation <= 7; orientation++)
        {
            tile.BorderBitmasks[orientation, TileBorder.Top] = BitFunctions.ReverseBits(tile.BorderBitmasks[orientation - 1, TileBorder.Left], tile.Image.Width);
            tile.BorderBitmasks[orientation, TileBorder.Right] = tile.BorderBitmasks[orientation - 1, TileBorder.Top];
            tile.BorderBitmasks[orientation, TileBorder.Bottom] = BitFunctions.ReverseBits(tile.BorderBitmasks[orientation - 1, TileBorder.Right], tile.Image.Width);
            tile.BorderBitmasks[orientation, TileBorder.Left] = tile.BorderBitmasks[orientation - 1, TileBorder.Bottom];
        }
    }
}
