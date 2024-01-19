using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Year2023.Day09;

internal class History
{
    public List<long> Sequence { get; private set; } = [];

    public long GetNextValue()
    {
        return BuildSequences().Sum(x => x[^1]);
    }

    public long GetPreviousValue()
    {
        return BuildSequences().Reverse().Aggregate(0L, (extrapolatedValue, sequence) => sequence[0] - extrapolatedValue);
    }

    public static History Parse(string input)
    {
        var history = new History
        {
            Sequence = [.. input.SelectToLongs(' ')]
        };

        return history;
    }

    public IEnumerable<List<long>> BuildSequences()
    {
        var sequences = new List<List<long>>
        {
            Sequence
        };

        yield return Sequence;

        while (sequences[^1].Exists(value => value != 0))
        {
            var nextSequence = sequences[^1]
                .Skip(1)
                .Select((value, index) => value - sequences[^1][index])
                .ToList();

            sequences.Add(nextSequence);

            yield return nextSequence;
        }
    }
}
