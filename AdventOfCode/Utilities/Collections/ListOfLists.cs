namespace AdventOfCode.Utilities.Collections;
internal class ListOfLists<T> where T : notnull
{
    private readonly int _capacity;
    private readonly List<List<T>> _lists = [];

    public ListOfLists(int capacity)
    {
        _capacity = capacity;
    }

    public void Add(T item)
    {
        if (_lists.Count == 0 || _lists[^1].Count == _capacity)
        {
            _lists.Add(new List<T>(_capacity));
        }

        _lists[^1].Add(item);
    }

    public ListOfListsIndex Find(Predicate<T> match, out T? result)
    {
        var index = 0;

        for (var listIndex = 0; listIndex < _lists.Count; listIndex++)
        {
            var subList = _lists[listIndex];

            var indexInList = subList.FindIndex(match);
            if (indexInList >= 0)
            {
                result = subList[indexInList];
                return new ListOfListsIndex(listIndex, indexInList);
            }

            index += subList.Count;
        }

        result = default;
        return new ListOfListsIndex(-1, -1);
    }

    public T this[ListOfListsIndex index]
    {
        get => _lists[index.ListIndex][index.IndexInList];
        set => throw new NotImplementedException();
    }

    public void RemoveAt(ListOfListsIndex index)
    {
        if (_lists[index.ListIndex].Count > 0)
        {
            _lists[index.ListIndex].RemoveAt(index.IndexInList);
        }
        else
        {
            _lists.RemoveAt(index.ListIndex);
        }
    }
}

internal sealed class ListOfListsIndex
{
    public int ListIndex { get; }

    public int IndexInList { get; }

    public bool IsValid => ListIndex >= 0 && IndexInList >= 0;

    public ListOfListsIndex(int listIndex, int indexInList)
    {
        ListIndex = listIndex;
        IndexInList = indexInList;
    }
}