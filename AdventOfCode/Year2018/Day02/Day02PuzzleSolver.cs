using AdventOfCode.Framework.Puzzle;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2018.Day02;

[Puzzle(2018, 2, "Inventory Management System")]
public class Day02PuzzleSolver : IPuzzleSolver
{
    private string[] _boxIds = [];

    public void ParseInput(string[] inputLines)
    {
        _boxIds = inputLines;
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var twoLetterBoxesCount = 0;
        var threeLetterBoxesCount = 0;

        foreach (string boxId in _boxIds)
        {
            var counts = new int['z' - 'a' + 1];

            foreach (var letter in boxId)
            {
                counts[letter - 'a']++;
            }

            if (Array.Exists(counts, x => x == 2))
            {
                twoLetterBoxesCount++;
            }

            if (Array.Exists(counts, x => x == 3))
            {
                threeLetterBoxesCount++;
            }
        }

        var answer = twoLetterBoxesCount * threeLetterBoxesCount;

        return new PuzzleAnswer(answer, 6696);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        foreach (var pair in _boxIds.Pairs())
        {
            var commonLetters = Enumerable.Range(0, pair.First.Length)
                .Where(letterIndex => pair.First[letterIndex] == pair.Second[letterIndex])
                .Select(letterIndex => pair.First[letterIndex])
                .ToArray();

            if (pair.First.Length - commonLetters.Length == 1) // 1 different letter
            {
                var answer = new string(commonLetters);
                return new PuzzleAnswer(answer, "bvnfawcnyoeyudzrpgslimtkj");
            }
        }

        return new PuzzleAnswer("ERROR", "ERROR");
    }
}