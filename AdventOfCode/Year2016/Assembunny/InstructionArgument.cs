namespace AdventOfCode.Year2016.Assembunny;

internal class InstructionArgument
{
    public int? Value { get; private set; }
    public int? Register { get; private set; }

    public static InstructionArgument Parse(string input)
    {
        if (int.TryParse(input, out int value))
        {
            return new InstructionArgument
            {
                Value = value,
            };
        }
        else
        {
            return new InstructionArgument
            {
                Register = input[0] - 'a',
            };
        }
    }
}
