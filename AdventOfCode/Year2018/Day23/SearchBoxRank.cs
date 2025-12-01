namespace AdventOfCode.Year2018.Day23;

internal readonly struct SearchBoxRank : IComparable<SearchBoxRank>
{
    public readonly long NanobotsCount { get; }
    public readonly long Distance { get; }
    public readonly long Volume { get; }

    public SearchBoxRank(long nanobotsCount, long distance, long volume)
    {
        NanobotsCount = nanobotsCount;
        Distance = distance;
        Volume = volume;
    }

    public int CompareTo(SearchBoxRank other)
    {
        // If one region contains more nanobots than the other, then rank it higher
        var nanobotsCountComparison = NanobotsCount.CompareTo(other.NanobotsCount);
        if (nanobotsCountComparison != 0)
        {
            return -1 * nanobotsCountComparison; // More is better
        }

        // If one region is close to the origin, then rank it higher
        var distanceComparison = Distance.CompareTo(other.Distance);
        if (distanceComparison != 0)
        {
            return distanceComparison;
        }

        // Rank the smaller region (by volume) higher
        return Volume.CompareTo(other.Volume);
    }

    public static bool operator ==(SearchBoxRank left, SearchBoxRank right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SearchBoxRank coordinate1, SearchBoxRank coordinate2)
    {
        return !coordinate1.Equals(coordinate2);
    }

    public bool Equals(SearchBoxRank other)
    {
        return NanobotsCount == other.NanobotsCount &&
               Distance == other.Distance &&
               Volume == other.Volume;
    }

    public override bool Equals(object? obj)
    {
        return obj is SearchBoxRank priority && Equals(priority);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NanobotsCount, Distance, Volume);
    }

    public static bool operator <(SearchBoxRank left, SearchBoxRank right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(SearchBoxRank left, SearchBoxRank right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(SearchBoxRank left, SearchBoxRank right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(SearchBoxRank left, SearchBoxRank right)
    {
        return left.CompareTo(right) >= 0;
    }
}
