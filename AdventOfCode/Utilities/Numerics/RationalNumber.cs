using AdventOfCode.Utilities.Mathematics;
using System.Numerics;

namespace AdventOfCode.Utilities.Numerics;

internal struct RationalNumber
{
    public static readonly RationalNumber Zero = new(0, 1);
    public static readonly RationalNumber One = new(1, 1);

    private BigInteger? _numerator;
    public BigInteger Numerator
    {
        readonly get => _numerator ?? 0;
        set => _numerator = value;
    }

    private BigInteger? _denominator;
    public BigInteger Denominator
    {
        readonly get => _denominator ?? 1;
        set => _denominator = value;
    }

    public readonly long LongValue
    {
        get
        {
            var bigValue = Numerator / Denominator;

            if (bigValue >= long.MinValue && bigValue <= long.MaxValue)
            {
                return (long)bigValue;
            }
            else
            {
                throw new InvalidOperationException("Value is too small/large for int64");
            }
        }
    }

    public readonly bool IsWholeNumber => Denominator == 1;

    public RationalNumber(long number) : this(number, 1L) { }

    public RationalNumber(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
        {
            throw new ArgumentException("Denominator cannot be zero.");
        }

        var numeratorSign = numerator switch
        {
            var n when n > BigInteger.Zero => 1,
            var n when n < BigInteger.Zero => -1,
            _ => 0,
        };

        var denominatorSign = denominator switch
        {
            var n when n > BigInteger.Zero => 1,
            var n when n < BigInteger.Zero => -1,
            _ => 0,
        };

        if (numerator == 0)
        {
            denominator = 1;
        }
        else if (numeratorSign != denominatorSign)
        {
            numerator = -BigInteger.Abs(numerator);
        }
        else
        {
            numerator = BigInteger.Abs(numerator);
        }

        denominator = BigInteger.Abs(denominator);

        if (denominator != 1)
        {
            var gcd = MathFunctions.GreatestCommonDivisor(BigInteger.Abs(numerator), BigInteger.Abs(denominator));
            if (gcd <= denominator)
            {
                Numerator = numerator / gcd;
                Denominator = denominator / gcd;

                return;
            }
        }

        Numerator = numerator;
        Denominator = denominator;
    }

    public readonly RationalNumber Ceiling()
    {
        if (IsWholeNumber)
        {
            return this;
        }

        if (this > Zero)
        {
            return new RationalNumber(LongValue + 1);
        }

        return new RationalNumber(LongValue);
    }

    public readonly RationalNumber Floor()
    {
        if (IsWholeNumber)
        {
            return this;
        }

        if (this > Zero)
        {
            return new RationalNumber(LongValue);
        }

        return new RationalNumber(LongValue - 1);
    }

    public static RationalNumber operator +(RationalNumber number1, RationalNumber number2)
    {
        var commonDenominator = number1.Denominator * number2.Denominator;

        var number1Numerator = number1.Numerator * number2.Denominator;
        var number2Numerator = number2.Numerator * number1.Denominator;

        return new RationalNumber(number1Numerator + number2Numerator, commonDenominator);
    }

    public static RationalNumber operator -(RationalNumber number1, RationalNumber number2)
    {
        var commonDenominator = number1.Denominator * number2.Denominator;

        var number1Numerator = number1.Numerator * number2.Denominator;
        var number2Numerator = number2.Numerator * number1.Denominator;

        return new RationalNumber(number1Numerator - number2Numerator, commonDenominator);
    }

    public static RationalNumber operator *(RationalNumber number1, RationalNumber number2)
    {
        return new RationalNumber(number1.Numerator * number2.Numerator, number1.Denominator * number2.Denominator);
    }

    public static RationalNumber operator /(RationalNumber number1, RationalNumber number2)
    {
        return new RationalNumber(number1.Numerator * number2.Denominator, number1.Denominator * number2.Numerator);
    }

    public static bool operator <(RationalNumber number1, RationalNumber number2)
    {
        return number1.Numerator * number2.Denominator < number2.Numerator * number1.Denominator;
    }

    public static bool operator <=(RationalNumber number1, RationalNumber number2)
    {
        return number1.Numerator * number2.Denominator <= number2.Numerator * number1.Denominator;
    }

    public static bool operator >(RationalNumber number1, RationalNumber number2)
    {
        return number1.Numerator * number2.Denominator > number2.Numerator * number1.Denominator;
    }

    public static bool operator >=(RationalNumber number1, RationalNumber number2)
    {
        return number1.Numerator * number2.Denominator >= number2.Numerator * number1.Denominator;
    }

    public static bool operator ==(RationalNumber number1, RationalNumber number2)
    {
        return number1.Numerator == number2.Numerator && number1.Denominator == number2.Denominator;
    }

    public static bool operator !=(RationalNumber number1, RationalNumber number2)
    {
        return number1.Numerator != number2.Numerator || number1.Denominator != number2.Denominator;
    }


    public override bool Equals(object? obj)
    {
        return obj is RationalNumber other && this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }

    public override readonly string ToString()
    {
        if (Numerator == Denominator)
        {
            return "1";
        }
        else if (Denominator == 1)
        {
            return string.Format("{0}", Numerator);
        }
        else if (Numerator < Denominator)
        {
            if (Numerator == 0)
                return "0";
            else
                return string.Format("{0}/{1}", Numerator, Denominator);
        }
        else
        {
            return string.Format("{0} {1}/{2}", Numerator / Denominator, Numerator % Denominator, Denominator);
        }
    }
}
