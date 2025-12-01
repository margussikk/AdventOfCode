namespace AdventOfCode.Year2017.Day18;

internal class Operand
{
    public bool IsRegister { get; private set; }

    public int Register { get; private set; }
    public int Value { get; private set; }

    public static Operand Parse(string input)
    {
        var operand = new Operand();

        if (char.IsLetter(input[0]))
        {
            operand.IsRegister = true;
            operand.Register = input[0] - 'a';
        }
        else
        {
            operand.Value = int.Parse(input);
        }

        return operand;
    }
}
