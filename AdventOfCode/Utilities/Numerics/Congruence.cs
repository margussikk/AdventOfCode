namespace AdventOfCode.Utilities.Numerics;

internal class Congruence(long modulus, long reminder)
{
    public long Modulus { get; } = modulus;
    public long Reminder { get; } = reminder;
}
