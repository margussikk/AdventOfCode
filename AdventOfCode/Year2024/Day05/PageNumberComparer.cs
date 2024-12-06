namespace AdventOfCode.Year2024.Day05;

internal class PageNumberComparer : IComparer<int>
{
    private readonly ILookup<int, int> _pageOrderingRules;

    public PageNumberComparer(ILookup<int, int> pageOrderingRules)
    {
        _pageOrderingRules = pageOrderingRules;
    }

    public int Compare(int first, int second)
    {
        if (first == second)
        {
            return 0;
        }

        if (_pageOrderingRules[first].Contains(second))
        {
            return -1;
        }

        return 1;
    }
}
