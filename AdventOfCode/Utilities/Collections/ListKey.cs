namespace AdventOfCode.Utilities.Collections;

internal readonly struct ListKey<T>
{
    private readonly int _hashCode;

    public IList<T> Data { get; }

    public ListKey(IList<T> data)
    {
        Data = data;

        var hashCode = new HashCode();
        foreach (var item in Data)
        {
            hashCode.Add(item);
        }

        _hashCode = hashCode.ToHashCode();
    }

    public override readonly bool Equals(object? obj) => obj is ListKey<T> o && Equals(o);

    public readonly bool Equals(ListKey<T> other)
    {
        return Data.SequenceEqual(other.Data);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
}
