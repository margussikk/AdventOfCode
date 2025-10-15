namespace AdventOfCode.Year2015.Day12;

internal class ObjectElement : Element
{
    public List<PropertyElement> Properties { get; } = [];

    public override int SumOfNumbers(bool notRed)
    {
        if (notRed && Properties.Any(p => p.Element is StringElement { Value: "red" }))
        {
            return 0;
        }

        return Properties.Sum(p => p.SumOfNumbers(notRed));
    }

    public static ObjectElement Parse(ref ReadOnlySpan<char> span)
    {
        var objectElement = new ObjectElement();

        ExpectAndSkip(ref span, '{');

        var processed = false;
        while (!processed)
        {
            var property = new PropertyElement();

            ExpectAndSkip(ref span, '"');

            var endIndex = span.IndexOf('"');
            property.Name = new string(span[..endIndex]);
            span = span[endIndex..];

            ExpectAndSkip(ref span, '"');
            ExpectAndSkip(ref span, ':');

            property.Element = span[0] switch
            {
                '{' => ObjectElement.Parse(ref span),
                '[' => ArrayElement.Parse(ref span),
                '"' => StringElement.Parse(ref span),
                '-' or >= '0' and <= '9' => NumberElement.Parse(ref span),
                _ => throw new InvalidOperationException($"Unexpected character {span[0]}")
            };

            objectElement.Properties.Add(property);

            if (span[0] is not (',' or '}'))
            {
                throw new InvalidOperationException($"Unexpected character {span[0]}");
            }

            processed = span[0] == '}';
            span = span[1..];
        }

        return objectElement;
    }
}
