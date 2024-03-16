using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Utilities.Numerics;

// Adapted from https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
internal static class ChineseRemainderTheoremSolver
{
    public static long Solve(List<Congruence> congruences)
    {
        var prod = congruences.Aggregate(1L, (i, j) => i * j.Modulus);
        var sum = 0L;

        foreach(var congruence in congruences)
        {
            var p = prod / congruence.Modulus;
            sum += congruence.Reminder * (long)MathFunctions.ModularMultiplicativeInverse(p, congruence.Modulus) * p;
        }

        return sum % prod;
    }
}
