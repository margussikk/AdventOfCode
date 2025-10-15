namespace AdventOfCode.Year2015.Day12;

internal class ArrayElement : Element
{
    public List<Element> Elements { get; } = [];

    public override int SumOfNumbers(bool notRed)
    {
        return Elements.Sum(e => e.SumOfNumbers(notRed));
    }

    public static ArrayElement Parse(ref ReadOnlySpan<char> span)
    {
        var arrayElement = new ArrayElement();

        ExpectAndSkip(ref span, '[');

        var processed = false;
        while (!processed)
        {
            Element element = span[0] switch
            {
                '{' => ObjectElement.Parse(ref span),
                '[' => ArrayElement.Parse(ref span),
                '"' => StringElement.Parse(ref span),
                '-' or >= '0' and <= '9' => NumberElement.Parse(ref span),
                _ => throw new InvalidOperationException($"Unexpected character {span[0]}")
            };

            arrayElement.Elements.Add(element);

            if (span[0] is not (',' or ']'))
            {
                throw new InvalidOperationException($"Unexpected character {span[0]}");
            }

            processed = span[0] == ']';
            span = span[1..];
        }

        return arrayElement;
    }
}
