using System.Numerics;

namespace AdventOfCode.Utilities.Mathematics;

internal static class MathFunctions
{
    public static T Modulo<T>(T first, T second) where T : INumber<T>
    {
        var c = first % second;
        return c < T.Zero ? c + second : c;
    }

    public static T GreatestCommonDivisor<T>(T first, T second) where T : INumber<T>
    {
        while (second != T.Zero)
        {
            var temp = second;
            second = first % second;
            first = temp;
        }

        return first;
    }

    public static T LeastCommonMultiple<T>(T first, T second) where T : INumber<T>
        => first / GreatestCommonDivisor(first, second) * second;

    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);

    public static long Factorial(int number)
    {
        long result = number;

        for (var i = 1; i < number; i++)
        {
            result *= i;
        }

        return result;
    }


    /// <summary>
    /// Extended Euclidean Algorithm (EEA) to obtain the modular multiplicative inverse of a mod m, m does not have to be a prime
    /// If modulus is a prime, Fermat's Little Theorem can be used and then result = BigInteger.ModPow(a, m - 2, m);
    /// </summary>
    public static BigInteger ModularMultiplicativeInverse(BigInteger a, BigInteger modulus)
    {
        BigInteger t = new(0), newt = new(1);
        BigInteger r = modulus, newr = a;

        while (newr != 0)
        {
            var quotient = r / newr;
            (t, newt) = (newt, t - quotient * newt);
            (r, newr) = (newr, r - quotient * newr);
        }

        if (r > 1)
        {
            throw new InvalidOperationException($"{a} is not invertible mod {modulus}");
        }

        if (t < 0)
        {
            t += modulus;
        }

        return t;
    }

    public static BigInteger ModularGeometricSum(BigInteger a, BigInteger r, BigInteger exponent, BigInteger modulus)
    {
        // The partial sum of a geometric series is given by: s = a * (1 - r^n) / (1 - r)
        // Under modulo m, this becomes: s = a * (1 - r^n) * ModInv(1 - r) mod m
        //
        var numerator = a * (1 - BigInteger.ModPow(value: r, exponent: exponent, modulus: modulus));
        var invDenominator = ModularMultiplicativeInverse(a: 1 - r, modulus: modulus);

        // Note, using the modular multiplicative inverse, we now multiply the numerator and the
        // inverted denominator
        //
        var partialSum = numerator * invDenominator;
        var inRange = Modulo(partialSum, modulus);

        return inRange;
    }
}
