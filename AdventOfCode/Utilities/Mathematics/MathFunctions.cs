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

    public static long Power10(int number)
    {
        long power = 1;

        for (int i = 0; i < number; i++)
        {
            power *= 10;
        }

        return power;
    }

    public static List<long> Divisors(long number)
    {
        var divisors = new List<long>();

        for (int i = 1; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
            {
                // If divisors are equal, take only one
                if (number / i == i)
                {
                    divisors.Add(i);
                }
                // Otherwise take both
                else
                {
                    divisors.Add(i);
                    divisors.Add(number / i);
                }
            }
        }

        divisors.Sort();

        return divisors;
    }

    public static bool IsPrime(long number)
    {
        var s = Math.Sqrt(number);

        for (var i = 2; i <= s; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }

        return number != 1;
    }

    public static Dictionary<long, long> PrimeFactors(long number)
    {
        if (number == 1)
        {
            return new Dictionary<long, long>
            {
                [1] = 1
            };
        }

        var primeFactors = new List<long>();

        // Get the number of 2s that divide n
        while (number % 2 == 0)
        {
            primeFactors.Add(2);
            number /= 2;
        }

        // n must be odd at this point. so we can skip one element
        // (note i = i + 2)
        for (var i = 3; i * i <= number; i += 2)
        {
            // while i divides n, append i and divide n
            while (number % i == 0)
            {
                primeFactors.Add(i);
                number /= i;
            }
        }

        // This condition is to handle the case when n is a prime number
        // greater than 2
        if (number > 2)
        {
            primeFactors.Add(number);
        }

        return primeFactors
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => g.LongCount());
    }

    public static long SumOfDivisors(long number)
    {
        var totalSum = 1L;

        foreach (var item in PrimeFactors(number))
        {
            var sum = 1L;
            var power = 1L;
            for (var k = 0; k < item.Value; k++)
            {
                power *= item.Key;
                sum += power;
            }

            totalSum *= sum;
        }

        return totalSum;
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

    public static int Fibonacci(int n)
    {
        if (n <= 0) return 0;
        if (n == 1) return 1;

        int prev = 0, current = 1, next = 0;
        for (int i = 2; i <= n; i++)
        {
            next = prev + current;
            prev = current;
            current = next;
        }
        return next;
    }

    public static int LargestPower(int number, int @base)
    {
        return Convert.ToInt32(Math.Pow(@base, Math.Floor(Math.Log(number, @base))));
    }
}
