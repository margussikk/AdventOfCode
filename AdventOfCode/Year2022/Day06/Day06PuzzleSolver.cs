using AdventOfCode.Framework.Puzzle;

namespace AdventOfCode.Year2022.Day06;

[Puzzle(2022, 6, "Tuning Trouble")]
public class Day06PuzzleSolver : IPuzzleSolver
{
    private string _dataStream = string.Empty;

    public void ParseInput(string[] inputLines)
    {
        _dataStream = inputLines[0];
    }

    public PuzzleAnswer GetPartOneAnswer()
    {
        var answer = GetCharactersProcessed(4);

        return new PuzzleAnswer(answer, 1625);
    }

    public PuzzleAnswer GetPartTwoAnswer()
    {
        var answer = GetCharactersProcessed(14);

        return new PuzzleAnswer(answer, 2250);
    }

    private int GetCharactersProcessed(int windowSize)
    {
        var counters = new int['z' - 'a' + 1];
        var uniqueCounter = 0;

        for (var index = 0; index < _dataStream.Length && uniqueCounter < windowSize; index++)
        {
            if (index >= windowSize)
            {
                var oldCharacter = _dataStream[index - windowSize] - 'a';
                counters[oldCharacter]--;
                if (counters[oldCharacter] == 0)
                {
                    uniqueCounter--;
                }
            }

            var newCharacter = _dataStream[index] - 'a';
            counters[newCharacter]++;
            if (counters[newCharacter] != 1) continue;
            
            uniqueCounter++;
            if (uniqueCounter == windowSize)
            {
                return index + 1;
            }
        }

        return -1;
    }
}