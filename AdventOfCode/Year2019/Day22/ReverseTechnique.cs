using AdventOfCode.Utilities.Mathematics;
using System.Numerics;

namespace AdventOfCode.Year2019.Day22;

internal class ReverseTechnique(long deckSize)
{
    public BigInteger Increment { get; private set; } = 1;
    public BigInteger Offset { get; private set; } = 0;
    public long DeckSize { get; } = deckSize;

    public void Combine(Technique technique)
    {
        if (technique is DealIntoNewStackTechnique)
        {
            Increment = -Increment;
            Offset = -Offset -1;
        }
        else if (technique is CutCardsTechnique cutCardsTechnique)
        {
            Offset += cutCardsTechnique.Amount;
        }
        else if (technique is DealWithIncrementTechnique dealWithIncrementTechnique)
        {
            var modInverse = MathFunctions.ModularMultiplicativeInverse(dealWithIncrementTechnique.Increment, DeckSize);

            Increment *= modInverse;
            Offset *= modInverse;
        }

        Increment = MathFunctions.Modulo(Increment, DeckSize);
        Offset = MathFunctions.Modulo(Offset, DeckSize);
    }

    public long Apply(long index, long times)
    {
        // After one time it is Increment * index + Offset
        // After two times it is Increment * (Increment * index + Offset) + Offset = Increment^2*index + Increment*Offset + Offset
        // After three times it is Increment^3*index + Increment^2*Offset + Increment*Offset + Offset
        // After N times it is Increment^N + Offset*(1 + Increment + Increment^2 ... Increment^N) = 
        // Increment^N + Offset * (1 - Increment^N) / (1 - Increment)

        var increment = BigInteger.ModPow(Increment, times, DeckSize);
        var offset = MathFunctions.ModularGeometricSum(Offset, Increment, times, DeckSize);

        return (long)MathFunctions.Modulo(increment * index + offset, DeckSize);       
    }
}
