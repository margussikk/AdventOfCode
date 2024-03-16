namespace AdventOfCode.Year2019.IntCode;

internal class IntCodeProgram
{
    public List<long> Code { get; private set; } = [];

    public static IntCodeProgram Parse(string input)
    {
        return new IntCodeProgram
        {
            Code = input.Split(',')
                        .Select(long.Parse)
                        .ToList(),
        };
    }
}
