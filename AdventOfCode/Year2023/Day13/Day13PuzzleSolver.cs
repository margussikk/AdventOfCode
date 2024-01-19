using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Geometry;

namespace AdventOfCode.Year2023.Day13;

[Puzzle(2023, 13, "Point of Incidence")]
public class Day13PuzzleSolver : IPuzzleSolver
{
    private List<Grid<Terrain>> _terrainGrids = [];

    public void ParseInput(string[] inputLines)
    {
        _terrainGrids = inputLines.SelectToChunks()
                                  .Select(list => list.SelectToGrid(character => character switch
                                                  {
                                                      '.' => Terrain.Ash,
                                                      '#' => Terrain.Rock,
                                                      _ => throw new NotImplementedException()
                                                  }))
                                  .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _terrainGrids
            .Sum(grid => CountReflectedRows(grid, 0) * 100 + CountReflectedColumns(grid, 0));

        return new PuzzleAnswer(answer, 33047);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _terrainGrids
            .Sum(grid => CountReflectedRows(grid, 1) * 100 + CountReflectedColumns(grid, 1));

        return new PuzzleAnswer(answer, 28806);
    }

    private static int CountReflectedColumns(Grid<Terrain> grid, int smudgeCount)
    {
        var count = 0;

        for (var column1 = 0; column1 < grid.LastColumnIndex; column1++)
        {
            var column2 = column1 + 1;

            var differences = CountColumnDifferences(grid, column1, column2);
            if (differences <= smudgeCount) // Possible reflection
            {
                var maxReflectionLength = Math.Min(column1, grid.LastColumnIndex - column2); // Length towards closest edge
                for (var distance = 1; distance <= maxReflectionLength; distance++)
                {
                    differences += CountColumnDifferences(grid, column1 - distance, column2 + distance);
                    if (differences > smudgeCount)
                    {
                        break;
                    }
                }
            }

            if (differences == smudgeCount)
            {
                count += column1 + 1;
            }
        }

        return count;
    }

    private static int CountReflectedRows(Grid<Terrain> grid, int smudgeCount)
    {
        return CountReflectedColumns(grid.RotateCounterClockwise(), smudgeCount);
    }

    private static int CountColumnDifferences(Grid<Terrain> grid, int column1, int column2)
    {
        return grid.Column(column1)
                   .Zip(grid.Column(column2))
                   .Count(z => z.First.Object != z.Second.Object);
    }
}
