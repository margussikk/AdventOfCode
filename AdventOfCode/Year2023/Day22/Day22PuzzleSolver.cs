using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2023.Day22;

[Puzzle(2023, 22, "Sand Slabs")]
public class Day22PuzzleSolver : IPuzzleSolver
{
    private List<Brick> _bricks = [];

    public void ParseInput(string[] inputLines)
    {
        _bricks = inputLines
            .Select((line, index) => Brick.Parse(index + 1, line)) // "real" brick ids start at 1, 0 means ground
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var bricks = SettleBricks(_bricks);

        var answer = bricks
            .Count(b1 => b1.Supports.All(b2 => b2.SupportedBy.Count > 1)); // NB! TrueForAll returns true if list is empty

        return new PuzzleAnswer(answer, 434);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = 0;

        var bricks = SettleBricks(_bricks);
        foreach (var brick in bricks)
        {
            var fallenBrickIds = new HashSet<int>();
            var fallenBricksQueue = new Queue<Brick>();

            fallenBricksQueue.Enqueue(brick);
            while (fallenBricksQueue.TryDequeue(out var fallenBrick))
            {
                fallenBrickIds.Add(fallenBrick.Id);
                foreach (var supportedBrick in fallenBrick.Supports)
                {
                    if (supportedBrick.SupportedBy.Any(x => !fallenBrickIds.Contains(x.Id)))
                    {
                        // Brick which is supported by current brick is also supported by some other brick.
                        continue;
                    }

                    fallenBricksQueue.Enqueue(supportedBrick);
                }
            }

            answer += fallenBrickIds.Count - 1; // - 1 = Don't count ground brick
        }

        return new PuzzleAnswer(answer, 61209);
    }

    private static List<Brick> SettleBricks(List<Brick> bricks)
    {
        bricks =
        [
            .. bricks.Select(b => b.CleanClone())
                     .OrderBy(x => x.Start.Z)
        ];

        var maxX = bricks.Max(b => b.End.X);
        var maxY = bricks.Max(b => b.End.Y);

        // Keep track of the highest brick that's "open". Add pseudo ground bricks.
        var groundBrick = Brick.CreateGroundBrick(maxX, maxY);

        var stackedBricks = new Brick[maxX + 1, maxY + 1];
        for (var x = 0; x < stackedBricks.GetLength(0); x++)
        {
            for (var y = 0; y < stackedBricks.GetLength(1); y++)
            {
                stackedBricks[x, y] = groundBrick;
            }
        }

        foreach (var brick in bricks)
        {
            // Find how low brick can drop
            var maxHeight = long.MinValue;

            for (var x = brick.Start.X; x <= brick.End.X; x++)
            {
                for (var y = brick.Start.Y; y <= brick.End.Y; y++)
                {
                    maxHeight = long.Max(maxHeight, stackedBricks[x, y].End.Z);
                }
            }

            brick.DropToZ(maxHeight + 1);

            for (var x = brick.Start.X; x <= brick.End.X; x++)
            {
                for (var y = brick.Start.Y; y <= brick.End.Y; y++)
                {
                    var stackedBrick = stackedBricks[x, y];

                    if (stackedBrick.End.Z == maxHeight)
                    {
                        brick.SupportedBy.Add(stackedBrick);
                        stackedBrick.Supports.Add(brick);
                    }

                    stackedBricks[x, y] = brick;
                }
            }
        }

        return bricks;
    }
}
