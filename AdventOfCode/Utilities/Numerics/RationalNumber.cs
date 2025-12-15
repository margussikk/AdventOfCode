using AdventOfCode.Utilities.Mathematics;

namespace AdventOfCode.Utilities.Numerics;

internal struct RationalNumber
{
    public static readonly RationalNumber Zero = new(0, 1);
    public static readonly RationalNumber One = new(1, 1);

    private long? _numerator;
    public long Numerator
    {
        readonly get => _numerator ?? 0;
        set => _numerator = value;
    }

    private long? _denominator;
    public long Denominator
    {
        readonly get => _denominator ?? 1;
        set => _denominator = value;
    }

    public readonly long LongValue => Numerator / Denominator;

    public readonly bool IsWholeNumber => Denominator == 1;

    public RationalNumber(long number) : this(number, 1L) { }

    public RationalNumber(long numerator, long denominator)
    {
        if (denominator == 0)
        {
            throw new ArgumentException("Denominator cannot be zero.");
        }

        if (numerator == 0)
        {
            denominator = 1;
        }
        else if (long.Sign(numerator) != long.Sign(denominator))
        {
            numerator = -Math.Abs(numerator);
        }
        else
        {
            numerator = Math.Abs(numerator);
        }

        denominator = Math.Abs(denominator);

        if (denominator != 1)
        {
            var gcd = MathFunctions.GreatestCommonDivisor(Math.Abs(numerator), Math.Abs(denominator));
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
