namespace AdventOfCode.Year2016.Day07;

internal class IPAddress
{
    public string Address { get; private set; } = string.Empty;

    public bool SupportsTls()
    {
        var containsAbbaOutsideBrackets = false;
        var insideBrackets = false;

        var startIndex = 0;
        while (startIndex < Address.Length)
        {
            var endIndex = Address.IndexOf(insideBrackets ? ']' : '[', startIndex);
            if (endIndex == -1)
            {
                endIndex = Address.Length;
            }
            var segment = Address[startIndex..endIndex];

            if (ContainsAbba(segment))
            {
                if (insideBrackets)
                {
                    return false;
                }

                containsAbbaOutsideBrackets = true;
            }

            startIndex = endIndex + 1;
            insideBrackets = !insideBrackets;
        }

        return containsAbbaOutsideBrackets;
    }

    public bool SupportsSsl()
    {
        var abas = new HashSet<string>();
        var babs = new HashSet<string>();

        var insideBrackets = false;

        var startIndex = 0;
        while (startIndex < Address.Length)
        {
            var endIndex = Address.IndexOf(insideBrackets ? ']' : '[', startIndex);
            if (endIndex == -1)
            {
                endIndex = Address.Length;
            }
            var segment = Address[startIndex..endIndex];

            var sequences = GetAbaOrBabSequence(segment);
            if (insideBrackets)
            {
                sequences.ForEach(x => babs.Add(x));
            }
            else
            {
                sequences.ForEach(x => abas.Add(x));
            }

            startIndex = endIndex + 1;
            insideBrackets = !insideBrackets;
        }

        return abas.Select(aba => $"{aba[1]}{aba[0]}{aba[1]}")
                   .Intersect(babs)
                   .Any();
    }

    public static IPAddress Parse(string input)
    {
        return new IPAddress
        {
            Address = input
        };
    }

    private static bool ContainsAbba(string segment)
    {
        for (var i = 0; i < segment.Length - 3; i++)
        {
            if (segment[i] != segment[i + 1] &&
                segment[i] == segment[i + 3] &&
                segment[i + 1] == segment[i + 2])
            {
                return true;
            }
        }
        return false;
    }

    private static List<string> GetAbaOrBabSequence(string segment)
    {
        var results = new List<string>();

        for (var i = 0; i < segment.Length - 2; i++)
        {
            if (segment[i] != segment[i + 1] && segment[i] == segment[i + 2])
            {
                results.Add(segment.Substring(i, 3));
            }
        }

        return results;
    }
}
