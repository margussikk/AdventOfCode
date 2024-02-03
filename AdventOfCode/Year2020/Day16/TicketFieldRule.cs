using AdventOfCode.Utilities.Numerics;

namespace AdventOfCode.Year2020.Day16;

internal class TicketFieldRule
{
    public string FieldName {  get; private set; } = string.Empty;

    public NumberRange<int>[] Ranges { get; private set; } = [];

    public bool IsValid(int number)
    {
        return Array.Exists(Ranges, range => range.Contains(number));
    }

    public static TicketFieldRule Parse(string input)
    {
        var separators = new string[]
        {
            ": ", " or "
        };

        var splits = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        var ticketFieldRule = new TicketFieldRule
        {
            FieldName = splits[0],
            Ranges =
            [
                NumberRange<int>.Parse(splits[1]),
                NumberRange<int>.Parse(splits[2]),
            ] 
        };

        return ticketFieldRule;
    }
}
