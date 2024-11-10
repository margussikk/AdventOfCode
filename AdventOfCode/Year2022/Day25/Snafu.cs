using System.Text;

namespace AdventOfCode.Year2022.Day25;

internal class Snafu
{
    private static readonly char[] _characters = ['=', '-', '0', '1', '2'];
    public long Value { get; }

    public Snafu(long decimalValue)
    {
        Value = decimalValue;
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        if (Value >= 0)
        {
            var decimalValue = Value;
            do
            {
                var lastDecDigit = decimalValue % 5;
                if (lastDecDigit is >= 0 and <= 2) // 0, 1, 2
                {
                    stringBuilder.Insert(0, _characters[lastDecDigit + 2]);
                    decimalValue -= lastDecDigit;
                }
                else // 3, 4
                {
                    stringBuilder.Insert(0, _characters[lastDecDigit - 3]);
                    decimalValue += 5 - lastDecDigit;
                }

                if (decimalValue >= 5)
                {
                    decimalValue /= 5;
                }
            } while (decimalValue != 0);
        }
        else
        {
            throw new NotImplementedException();
        }

        return stringBuilder.ToString();
    }

    public static Snafu Parse(string line)
    {
        var value = 0L;

        foreach (var character in line)
        {
            value *= 5;
            value += Array.IndexOf(_characters, character) - 2;
        }

        var snafu = new Snafu(value);

        return snafu;
    }
}
