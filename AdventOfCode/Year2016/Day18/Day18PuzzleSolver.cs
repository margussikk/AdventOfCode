using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2016.Day18;

[Puzzle(2016, 18, "Like a Rogue")]
public class Day18PuzzleSolver : IPuzzleSolver
{
    private List<Tile> _inputTiles = [];

    public void ParseInput(string[] inputLines)
    {
        _inputTiles = [.. inputLines[0]
            .Select(c => c switch
            {
                '.' => Tile.Safe,
                '^' => Tile.Trap,
                _ => throw new InvalidOperationException($"Invalid input character: {c}")
            })];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetAnswer(40);

        return new PuzzleAnswer(answer, 2016);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetAnswer(400_000);

        return new PuzzleAnswer(answer, 19998750);
    }

    public int GetAnswer(int rows)
    {
        var answer = 0;

        var tiles = _inputTiles.ToArray();
        answer = tiles.Count(x => x == Tile.Safe);

        for (var row = 0; row < rows - 1; row++)
        {
            (tiles, var safeTiles) = GetNextTiles(tiles);
            answer += safeTiles;
        }

        return answer;
    }

    private static (Tile[], int) GetNextTiles(Tile[] tiles)
    {
        var safeTiles = 0;
        var nextTiles = new Tile[tiles.Length];

        for (var column = 0; column < tiles.Length; column++)
        {
            var left = column > 0 ? tiles[column - 1] : Tile.Safe;
            var center = tiles[column];
            var right = column < (tiles.Length - 1) ? tiles[column + 1] : Tile.Safe;

            if (left == Tile.Trap && center == Tile.Trap && right == Tile.Safe)
            {
                nextTiles[column] = Tile.Trap;
            }
            else if (left == Tile.Safe && center == Tile.Trap && right == Tile.Trap)
            {
                nextTiles[column] = Tile.Trap;
            }
            else if (left == Tile.Trap && center == Tile.Safe && right == Tile.Safe)
            {
                nextTiles[column] = Tile.Trap;
            }
            else if (left == Tile.Safe && center == Tile.Safe && right == Tile.Trap)
            {
                nextTiles[column] = Tile.Trap;
            }
            else
            {
                nextTiles[column] = Tile.Safe;
                safeTiles++;
            }
        }

        return (nextTiles, safeTiles);
    }
}