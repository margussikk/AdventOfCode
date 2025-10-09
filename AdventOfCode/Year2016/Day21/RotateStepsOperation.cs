namespace AdventOfCode.Year2016.Day21;

internal class RotateStepsOperation : Operation
{
    public int Steps { get; set; }

    public override void Scramble(char[] letters)
    {
        Rotate(letters, Steps);
    }

    public override void Unscramble(char[] letters)
    {
        Rotate(letters, -Steps);
    }

    private static void Rotate(char[] letters, int steps)
    {
        var clone = letters.ToArray();

        for (var position = 0; position < letters.Length; position++)
        {
            letters[position] = clone[(letters.Length + position - steps) % letters.Length];
        }
    }
}
