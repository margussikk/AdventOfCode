using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day14;

[Puzzle(2023, 14, "Parabolic Reflector Dish")]
public class Day14PuzzleSolver : IPuzzleSolver
{
    private Grid<Item> _platform = new(0, 0);

    public void ParseInput(List<string> inputLines)
    {
        _platform = inputLines.SelectToGrid(character => character switch
        {
            '.' => Item.EmptySpace,
            'O' => Item.RoundedRock,
            '#' => Item.CubeShapedRock,
            _ => throw new InvalidOperationException()
        });
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var platform = _platform.Clone();

        TiltToNorth(platform);

        var answer = CalculateTotalLoad(platform);

        return new PuzzleAnswer(answer, 109833);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;
        var totalCycles = 1_000_000_000;
        var platformCycles = new Dictionary<int, int>();
        var totalLoads = new List<int>();

        var platform = _platform.Clone();

        for (var cycle = 0; cycle < totalCycles; cycle++)
        {
            var hashCode = platform.GetHashCode();
            if (platformCycles.TryGetValue(hashCode, out var cycleStart))
            {
                var cycleLength = cycle - cycleStart;
                var reminder = (totalCycles - cycle) % cycleLength;

                answer = totalLoads[cycleStart + reminder];
                break;
            }
            else
            {
                platformCycles[hashCode] = cycle;
            }

            totalLoads.Add(CalculateTotalLoad(platform));

            platform = SpinCycle(platform);
        }

        return new PuzzleAnswer(answer, 99875);
    }


    private static Grid<Item> SpinCycle(Grid<Item> platform)
    {
        // Instead of tilting to west, south and east we rotate the grid and always tilt to the north

        // North
        TiltToNorth(platform);

        // West
        platform = platform.RotateClockwise();
        TiltToNorth(platform);

        // South
        platform = platform.RotateClockwise();
        TiltToNorth(platform);

        // East
        platform = platform.RotateClockwise();
        TiltToNorth(platform);

        // Rotate back to original direction
        platform = platform.RotateClockwise();

        return platform;
    }

    private static void TiltToNorth(Grid<Item> platform)
    {
        foreach(var cell in platform.Where(c => c.Object == Item.RoundedRock))
        {
            var rolledToRow = cell.Coordinate.Row;
            while (rolledToRow > 0 && platform[rolledToRow - 1, cell.Coordinate.Column] == Item.EmptySpace)
            {
                rolledToRow--;
            }

            if (rolledToRow != cell.Coordinate.Row)
            {
                platform[rolledToRow, cell.Coordinate.Column] = Item.RoundedRock;
                platform[cell.Coordinate.Row, cell.Coordinate.Column] = Item.EmptySpace;
            }
        }
    }

    private static int CalculateTotalLoad(Grid<Item> platform)
    {
        return Enumerable.Range(0, platform.Height)
            .Sum(row => (platform.Height - row) *
                platform.Row(row)
                        .Count(cell => cell.Object == Item.RoundedRock));
    }

    private static void Print(Grid<Item> platform)
    {
        for (var row = 0; row < platform.Height; row++)
        {
            for (var column = 0; column < platform.Width; column++)
            {
                var letter = platform[row, column] switch
                {
                    Item.EmptySpace => '.',
                    Item.RoundedRock => 'O',
                    Item.CubeShapedRock => '#',
                    _ => throw new NotImplementedException()
                };

                Console.Write(letter);
            }
            Console.WriteLine();
        }
    }
}
