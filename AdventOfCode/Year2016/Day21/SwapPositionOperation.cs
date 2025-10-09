namespace AdventOfCode.Year2016.Day21;

internal class SwapPositionOperation : Operation
{
    public int Position1 { get; init; }
    public int Position2 { get; init; }

    public override void Scramble(char[] letters)
    {
        (letters[Position1], letters[Position2]) = (letters[Position2], letters[Position1]);
    }

    public override void Unscramble(char[] letters)
    {
        Scramble(letters);
    }
}
