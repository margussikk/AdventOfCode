using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2017.Day04;

[Puzzle(2017, 4, "High-Entropy Passphrases")]
public class Day04PuzzleSolver : IPuzzleSolver
{
    private List<string[]> _passphrases = [];

    public void ParseInput(string[] inputLines)
    {
        _passphrases = inputLines.Select(s => s.Split(' ')).ToList();
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = _passphrases.Count(passphrase => passphrase.Distinct()
                                                                .Count() == passphrase.Length);

        return new PuzzleAnswer(answer, 451);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = _passphrases.Count(passphrase => passphrase.Select(SortString)
                                                                .Distinct()
                                                                .Count() == passphrase.Length);

        return new PuzzleAnswer(answer, 223);
    }

    private static string SortString(string input)
    {
        var characters = input.ToArray();
        Array.Sort(characters);
        return new string(characters);
    }
}