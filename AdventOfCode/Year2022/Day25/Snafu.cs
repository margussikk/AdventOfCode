using System.Text;

namespace AdventOfCode.Year2022.Day25;

internal class Snafu(long decimalValue)
{
    public long Value { get; private set; } = decimalValue;

    private static readonly char[] _characters = ['=', '-', '0', '1', '2'];

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        if (Value >= 0)
        {
            var decValue = Value;
            do
            {
                var lastDecDigit = decValue % 5;
                if (lastDecDigit >= 0 && lastDecDigit <= 2) // 0, 1, 2
                {
                    stringBuilder.Insert(0, _characters[lastDecDigit + 2]);
                    decValue -= lastDecDigit;
                }
                else // 3, 4
                {
                    stringBuilder.Insert(0, _characters[lastDecDigit - 3]);
                    decValue += (5 - lastDecDigit);
                }

                if (decValue >= 5)
                {
                    decValue /= 5;
                }
            } while (decValue != 0);
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
            value += (Array.IndexOf(_characters, character) - 2);
        }

        var snafu = new Snafu(value);

        return snafu;
    }
}
