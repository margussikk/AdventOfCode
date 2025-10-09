namespace AdventOfCode.Year2016.Day21;

internal class MovePositionOperation : Operation
{
    public int StartPosition { get; init; }
    public int EndPosition { get; init; }

    public override void Scramble(char[] letters)
    {
        var newLetters = letters.ToList();

        var letter = letters[StartPosition];
        newLetters.RemoveAt(StartPosition);

        if (EndPosition == newLetters.Count)
        {
            newLetters.Add(letter);
        }
        else
        {
            newLetters.Insert(EndPosition, letter);
        }

        for (var position = 0; position < newLetters.Count; position++)
        {
            letters[position] = newLetters[position];
        }
    }

    public override void Unscramble(char[] letters)
    {
        var newLetters = letters.ToList();

        var letter = letters[EndPosition];
        newLetters.RemoveAt(EndPosition);

        newLetters.Insert(StartPosition, letter);

        for (var position = 0; position < newLetters.Count; position++)
        {
            letters[position] = newLetters[position];
        }
    }
}
