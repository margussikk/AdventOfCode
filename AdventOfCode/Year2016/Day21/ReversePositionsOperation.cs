namespace AdventOfCode.Year2016.Day21;

internal class ReversePositionsOperation : Operation
{
    public int Position1 { get; init; }
    public int Position2 { get; init; }

    public override void Scramble(char[] letters)
    {
        var clone = letters.ToArray();

        for (var position = Position1; position <= Position2; position++)
        {
            letters[position] = clone[Position2 - (position - Position1)];
        }
    }

    public override void Unscramble(char[] letters)
    {
        Scramble(letters);
    }
}
