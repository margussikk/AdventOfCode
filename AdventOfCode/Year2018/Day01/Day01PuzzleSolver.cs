using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2018.Day01;

[Puzzle(2018, 1, "Chronal Calibration")]
public class Day01PuzzleSolver : IPuzzleSolver
{
    private List<int> _frequencyChanges = [];

    public void ParseInput(string[] inputLines)
    {
        _frequencyChanges = inputLines
            .Select(x => int.Parse(x.Replace("+", "")))
            .ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _frequencyChanges.Sum();

        return new PuzzleAnswer(answer, 529);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var reachedFrequencies = new HashSet<int>();

        var frequency = 0;
        var frequencyChangeIndex = 0;

        while (reachedFrequencies.Add(frequency))
        {
            frequency += _frequencyChanges[frequencyChangeIndex];
            frequencyChangeIndex = (frequencyChangeIndex + 1) % _frequencyChanges.Count;
        }

        return new PuzzleAnswer(frequency, 464);
    }
}