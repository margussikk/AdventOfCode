namespace AdventOfCode.Year2015.Day16;

internal class Sue
{
    public int Id { get; set; }
    public IDictionary<string, int> Properties { get; } = new Dictionary<string, int>();

    public static Sue Parse(string line)
    {
        var parts = line.Split([' ', ':', ','], StringSplitOptions.RemoveEmptyEntries);
        var sue = new Sue
        {
            Id = int.Parse(parts[1])
        };

        for (int i = 2; i < parts.Length; i += 2)
        {
            var property = parts[i];
            var value = int.Parse(parts[i + 1]);
            sue.Properties[property] = value;
        }
        return sue;
    }
}
