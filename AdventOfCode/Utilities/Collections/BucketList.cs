using System.Collections;

namespace AdventOfCode.Utilities.Collections;

internal class BucketList<T> : IList<T> where T : notnull
{
    public T this[int index]
    {
        get
        {
            var indexes = GetIndexes(index);
            return _buckets[indexes.BucketIndex][indexes.IndexInBucket];
        }
        set => throw new NotImplementedException();
    }

    public int Count => _buckets.Aggregate(0, (acc, curr) => acc + curr.Count);

    public bool IsReadOnly => throw new NotImplementedException();

    private readonly int _bucketSize;

    public readonly Dictionary<T, int> BucketIndexes = [];

    private readonly List<List<T>> _buckets = [[]];

    public BucketList(IEnumerable<T> items)
    {
        _bucketSize = 64;

        foreach (var item in items)
        {
            Add(item);
        }
    }


    public void Add(T item)
    {
        if (_buckets[^1].Count == _bucketSize)
        {
            _buckets.Add([]);
        }

        _buckets[^1].Add(item);
        BucketIndexes[item] = _buckets.Count - 1;
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(T item)
    {
        // Get index in own bucket
        var bucketIndex = BucketIndexes[item];
        var index = _buckets[bucketIndex].IndexOf(item);

        // Add previous bucket counts
        for (var i = 0; i < bucketIndex; i++)
        {
            index += _buckets[i].Count;
        }

        return index;
    }

    public void Insert(int index, T item)
    {
        var indexes = GetIndexes(index);

        BucketIndexes[item] = indexes.BucketIndex;
        _buckets[indexes.BucketIndex].Insert(indexes.IndexInBucket, item);
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        var indexes = GetIndexes(index);


        var item = _buckets[indexes.BucketIndex][indexes.IndexInBucket];

        BucketIndexes.Remove(item);
        _buckets[indexes.BucketIndex].RemoveAt(indexes.IndexInBucket);
    }

    public int FindIndex(Predicate<T> match)
    {
        var index = 0;

        foreach (var item in _buckets.SelectMany(bucket => bucket))
        {
            if (match(item))
            {
                return index;
            }

            index++;
        }

        return index;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    private BucketListIndexes GetIndexes(int index)
    {
        var currentIndex = 0;

        for (var bucketIndex = 0; bucketIndex < _buckets.Count; bucketIndex++)
        {
            var bucket = _buckets[bucketIndex];

            if (index >= currentIndex && index < currentIndex + bucket.Count)
            {
                var indexInBucket = index - currentIndex;
                return new BucketListIndexes(bucketIndex, indexInBucket);
            }

            currentIndex += bucket.Count;
        }

        throw new InvalidOperationException();
    }

    private sealed record BucketListIndexes(int BucketIndex, int IndexInBucket);
}
