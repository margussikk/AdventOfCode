namespace AdventOfCode.Utilities.Collections;
internal class ValuePair<T>
{
    public T First { get; }
    public T Second { get; }

    public ValuePair(T first, T second)
    {
        First = first;
        Second = second;
    }
}
