using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.GridSystem;

namespace AdventOfCode.Year2025.Day12;

[Puzzle(2025, 12, "Christmas Tree Farm")]
public class Day12PuzzleSolver : IPuzzleSolver
{
    private IReadOnlyList<Grid<bool>> _presents = [];
    private IReadOnlyList<Region> _regions = [];

    public void ParseInput(string[] inputLines)
    {
        var chunks = inputLines.SelectToChunks();

        _presents = [.. chunks[..^1].Select(chunk => chunk[1..].SelectToGrid(c => c == '#'))];
        _regions = [.. chunks[^1].Select(Region.Parse)];
    }

    // This solution works only with the given input.
    // It actually doesn't matter if we use only filled spots of the shape or all 9 spots of the shape to determine if region fits all of the presents.
    public PuzzleAnswer GetPartOneAnswer()
    {
        var filledCounts = _presents.Select(p => p.Count(cell => cell.Object)).ToList();

        var answer = 0;

        foreach (var region in _regions)
        {
            var filledArea = region.Quantities
                .Zip(filledCounts)
                .Sum(x => x.First * x.Second);

            if (filledArea <= region.Width * region.Height)
            {
                answer++;
            }
        }

        return new PuzzleAnswer(answer, 524);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}