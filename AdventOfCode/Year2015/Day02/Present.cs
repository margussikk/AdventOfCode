using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2015.Day02;

internal class Present
{
    public IReadOnlyList<int> Dimensions { get; private set; } = [];

    public int GetSquareFeetOfWrappingPaper()
    {
        var orderedDimensions = Dimensions.Order().ToArray();

        // area = 2*l*w + 2*w*h + 2*h*l
        var l = orderedDimensions[0];
        var w = orderedDimensions[1];
        var h = orderedDimensions[2];

        var area = 2 * l * w + 2 * w * h + 2 * h * l;
        var slack = l * w;

        return area + slack;
    }

    public int GetFeetOfRibbon()
    {
        var orderedDimensions = Dimensions.Order().ToArray();

        var wrapping = 2 * (orderedDimensions[0] + orderedDimensions[1]);
        var bow = orderedDimensions[0] * orderedDimensions[1] * orderedDimensions[2];

        return wrapping + bow;
    }

    public static Present Parse(string input)
    {
        return new Present
        {
            Dimensions = [.. input.SplitToNumbers<int>('x')]
        };
    }
}
