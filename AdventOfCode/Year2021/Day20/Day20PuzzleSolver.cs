using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2021.Day20;

[Puzzle(2021, 20, "Trench Map")]
public class Day20PuzzleSolver : IPuzzleSolver
{
    private bool[] _imageEnhancementAlgorithm = [];
    private BitGrid _image = new(0, 0);

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _imageEnhancementAlgorithm = chunks[0][0].Select(x => x == '#')
                                                 .ToArray();

        _image = chunks[1].SelectToBitGrid(character => character == '#');
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(2);

        return new PuzzleAnswer(answer, 4968);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(50);

        return new PuzzleAnswer(answer, 16793);
    }

    private int GetAnswer(int enhancementCount)
    {
        var defaultPixel = false;
        var grid = _image;

        for (var i = 0; i < enhancementCount; i++)
        {
            grid = Enhance(grid, _imageEnhancementAlgorithm, defaultPixel);
            defaultPixel = _imageEnhancementAlgorithm[defaultPixel ? 511 : 0];
        }

        return CountLitPixels(grid);
    }

    private static int CountLitPixels(BitGrid grid)
    {
        var litPixelsCount = 0;
        for (var row = 0; row < grid.Height; row++)
        {
            for (var column = 0; column < grid.Width; column++)
            {
                if (grid[row, column])
                {
                    litPixelsCount++;
                }
            }
        }

        return litPixelsCount;
    }

    private static BitGrid Enhance(BitGrid grid, bool[] algorithm, bool defaultPixel)
    {
        var newGrid = new BitGrid(grid.Height + 2, grid.Width + 2);

        for (var row = -1; row < grid.Height + 1; row++)
        {
            for (var column = -1; column < grid.Width + 1; column++)
            {
                var pixelNumber = GetPixelNumber(grid, row, column, defaultPixel);
                newGrid[row + 1, column + 1] = algorithm[pixelNumber];
            }
        }

        return newGrid;
    }

    private static int GetPixelNumber(BitGrid grid, int middleRow, int middleColumn, bool defaultPixel)
    {
        var pixelNumber = 0;
        for (var row = middleRow - 1; row <= middleRow + 1; row++)
        {
            for (var column = middleColumn - 1; column <= middleColumn + 1; column++)
            {
                var litPixel = grid.InBounds(row, column)
                    ? grid[row, column]
                    : defaultPixel;

                if (litPixel)
                {
                    pixelNumber = pixelNumber * 2 + 1;
                }
                else
                {
                    pixelNumber *= 2;
                }
            }
        }

        return pixelNumber;
    }

    private static void Print(BitGrid image)
    {
        for (var row = 0; row < image.Height; row++)
        {
            for (var col = 0; col < image.Width; col++)
            {
                Console.Write($"{(image[row, col] ? '#' : '.')}");
            }
            Console.WriteLine();
        }
    }
}