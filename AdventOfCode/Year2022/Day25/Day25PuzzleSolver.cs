using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day25;

[Puzzle(2022, 25, "Full of Hot Air")]
public class Day25PuzzleSolver : IPuzzleSolver
{
    private List<Snafu> Snafus = [];

    public void ParseInput(string[] inputLines)
    {
        Snafus = inputLines.Select(Snafu.Parse)
                           .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var sum = Snafus.Sum(x => x.Value);

        var sumSnafu = new Snafu(sum);

        var answer = sumSnafu.ToString();

        return new PuzzleAnswer(answer, "2-0-01==0-1=2212=100");
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        return new PuzzleAnswer("Merry Christmas", "Merry Christmas");
    }
}