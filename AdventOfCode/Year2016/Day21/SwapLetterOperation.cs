namespace AdventOfCode.Year2016.Day21;

internal class SwapLetterOperation : Operation
{
    public char Letter1 { get; init; }
    public char Letter2 { get; init; }

    public override void Scramble(char[] letters)
    {
        var position1 = Array.IndexOf(letters, Letter1);
        var position2 = Array.IndexOf(letters, Letter2);

        (letters[position1], letters[position2]) = (letters[position2], letters[position1]);
    }

    public override void Unscramble(char[] letters)
    {
        Scramble(letters);
    }
}
