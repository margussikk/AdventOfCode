namespace AdventOfCode.Year2019.Day22;

internal abstract class Technique
{
    public abstract List<int> ApplyFull(List<int> deck);

    public abstract long Apply(long index, long deckSize);

    public abstract long ApplyInverse(long index, long deckSize);

    public static Technique Parse(string input)
    {
        if (input == "deal into new stack")
        {
            return new DealIntoNewStackTechnique();
        }
        else if (input.StartsWith("cut "))
        {
            return new CutCardsTechnique(int.Parse(input["cut ".Length..]));
        }
        else if (input.StartsWith("deal with increment "))
        {
            return new DealWithIncrementTechnique(int.Parse(input["deal with increment ".Length..]));
        }

        throw new InvalidOperationException($"Failed to parse input '{input}'");
    }
}
