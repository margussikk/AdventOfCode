using System.Reflection;

namespace AdventOfCode.Framework.Puzzle;

public static class PuzzleDetailsProvider
{
    public static IReadOnlyList<int> GetYears()
    {
        return [.. BuildQuery()
            .Select(x => x.Year)
            .Distinct()
            .Order()];
    }

    public static IReadOnlyList<PuzzleDetails> GetByYear(int year)
    {
        return [.. BuildQuery()
            .Where(x => x.Year == year)
            .OrderBy(x => x.Day)];
    }

    public static IReadOnlyList<PuzzleDetails> GetByYearAndDays(int year, int[] days)
    {
        return [.. BuildQuery()
            .Where(x => x.Year == year && days.Contains(x.Day))
            .OrderBy(x => x.Day)];
    }


    private static IEnumerable<PuzzleDetails> BuildQuery()
    {
        return Assembly.GetExecutingAssembly()
                       .GetTypes()
                       .Where(type => !type.IsInterface && typeof(IPuzzleSolver).IsAssignableFrom(type))
                       .Select(type => (Type: type, Attribute: type.GetCustomAttribute<PuzzleAttribute>()))
                       .Where(x => x.Attribute != null)
                       .Select(x => new PuzzleDetails(x.Type, x.Attribute!.Year, x.Attribute.Day, x.Attribute.Name));
    }
}
